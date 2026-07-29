using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Inventory.API.Extensions
{
    public static class CompressionExtensions
    {
        public static IServiceCollection AddApiCompression(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;

                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();

                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                [
                    "application/json",
                    "application/xml",
                    "text/plain"
                ]);
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            return services;
        }
    }
}