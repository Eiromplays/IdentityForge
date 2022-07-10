using Eiromplays.IdentityServer.Application.Common.Caching;
using Eiromplays.IdentityServer.Application.Common.FileStorage;

namespace Eiromplays.IdentityServer.API.Services;

internal static class Startup
{
    internal static IServiceCollection AddCloudflareImagesStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudflareConfiguration = configuration.GetSection(nameof(CloudflareConfiguration)).Get<CloudflareConfiguration>();

        if (!cloudflareConfiguration.Enabled)
            return services;

        var descriptorToRemove = services.FirstOrDefault(s => s.ServiceType == typeof(IFileStorageService));
        if (descriptorToRemove is not null)
            services.Remove(descriptorToRemove);

        return services.AddTransient(typeof(IFileStorageService), typeof(CloudflareImagesStorageService));
    }
}