using Eiromplays.IdentityServer.Application.Common.Caching;
using Eiromplays.IdentityServer.Application.Common.FileStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Eiromplays.IdentityServer.Infrastructure.FileStorage;

internal static class Startup
{
    internal static IServiceCollection AddCloudflareImagesStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudflareConfiguration = configuration.GetSection(nameof(CloudflareConfiguration)).Get<CloudflareConfiguration>();

        if (cloudflareConfiguration?.Enabled == false)
            return services;

        var descriptorToRemove = services.FirstOrDefault(s => s.ServiceType == typeof(IFileStorageService));
        if (descriptorToRemove is not null)
            services.Remove(descriptorToRemove);

        return services.AddTransient(typeof(IFileStorageService), typeof(CloudflareImagesStorageService));
    }

    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        string staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        if (!Directory.Exists(staticFilesPath))
        {
            Directory.CreateDirectory(staticFilesPath);
        }

        return app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = new PathString("/Files")
        });
    }
}