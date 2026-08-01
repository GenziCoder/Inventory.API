namespace Inventory.API.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddAngularCors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //var origins = configuration
            //    .GetSection("AllowedOrigins")
            //    .Get<string[]>() ?? Array.Empty<string>();

            services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", policy =>
                {
                    policy.WithOrigins(
                        
                        "https://inventory-management-system-silk-ten.vercel.app",
                        "http://localhost:4200"
                        )
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}