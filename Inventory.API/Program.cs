using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using Inventory.API.Data;
using Inventory.API.Extensions;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Inventory.API.Mapping;
using Inventory.API.Middleware;
using Inventory.API.Repositories;
using Inventory.API.Services;
using Inventory.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var options = new WebApplicationOptions
        {
            Args = args
        };
        var builder = WebApplication.CreateBuilder(options);
        builder.Configuration.Sources.Clear();

        builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                 optional: true,
                 reloadOnChange: false)
    .AddEnvironmentVariables();


        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddAngularCors(builder.Configuration);
        builder.Services.AddApiCompression();

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, _) =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    Message = "Too many requests. Please try again later."
                });
            };

            options.AddFixedWindowLimiter("GlobalPolicy", policy =>
            {
                policy.PermitLimit = 2;
                policy.Window = TimeSpan.FromMinutes(1);
            });
        });
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("ReadPolicy", policy =>
            {
                policy.PermitLimit = 500;
                policy.Window = TimeSpan.FromMinutes(1);
            });

            options.AddFixedWindowLimiter("WritePolicy", policy =>
            {
                policy.PermitLimit = 100;
                policy.Window = TimeSpan.FromMinutes(1);
            });

            options.AddFixedWindowLimiter("LoginPolicy", policy =>
            {
                policy.PermitLimit = 5;
                policy.Window = TimeSpan.FromMinutes(5);
            });
        });
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            options.JsonSerializerOptions.NumberHandling =
            System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.Converters.Add(new DecimalJsonConverter());

        });

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Inventory Management API",
                Version = "v1",
                Description = "Inventory Management System API"
            });

            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Bearer token",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
                });
        });
        builder.Services.AddHealthChecks()
            .AddMySql(
                builder.Configuration.GetConnectionString("DefaultConnection")!,
                name: "MySQL Database",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "mysql" });
        builder.Services.AddAutoMapper(typeof(CategoryProfile));
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        builder.Services.AddScoped<IPurchaseService, PurchaseService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<ISaleService, SaleService>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IDashboardService, DashboardService>();
        builder.Services.AddScoped<IGlobalSearchService, GlobalSearchService>();
        builder.Services.AddScoped<IExportService, ExportService>();

        // Register DbContext
        //builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        //ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));
        });
        builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("Jwt"));
        var jwtSection = builder.Configuration.GetSection("Jwt");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;

            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSection["Issuer"],

                ValidAudience = jwtSection["Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSection["Key"]!))
            };
        });
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await DbInitializer.SeedAsync(context);
        }
        app.UseSerilogRequestLogging();
        app.UseGlobalExceptionMiddleware();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API v1");
            });
        }

        app.UseHttpsRedirection();
        
        app.UseResponseCompression();
        
        app.UseRateLimiter();

        app.UseCors("AngularPolicy");

       // app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Status = report.Status.ToString(),
                    TotalDuration = report.TotalDuration.TotalMilliseconds,
                    Checks = report.Entries.Select(x => new
                    {
                        Name = x.Key,
                        Status = x.Value.Status.ToString(),
                        Duration = x.Value.Duration.TotalMilliseconds,
                        x.Value.Description
                    })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            }
        }).DisableRateLimiting();
        app.MapHealthChecks("/health/live");
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("db")
        });
        app.Run();
    }
}