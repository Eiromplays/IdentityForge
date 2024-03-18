using IdentityForge.IdentityServer.Configurations;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace IdentityForge.IdentityServer.Extensions.Services;

public static class SwaggerServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetSwaggerSettings();

        if (swaggerSettings is null)
        {
            throw new ArgumentNullException(nameof(swaggerSettings), "Swagger settings are not configured.");
        }

        if (!swaggerSettings.Enable) return;

        services.AddSwaggerDocument(options =>
        {
            options.PostProcess = document =>
            {
                document.Info.Title = swaggerSettings.Title;
                document.Info.Description = swaggerSettings.Description;
                document.Info.Version = swaggerSettings.Version;
                document.Info.Contact = new OpenApiContact
                {
                    Name = swaggerSettings.ContactName,
                    Email = swaggerSettings.ContactEmail,
                    Url = swaggerSettings.ContactUrl
                };
                document.Info.License = new OpenApiLicense
                {
                    Name = swaggerSettings.LicenseName,
                    Url = swaggerSettings.LicenseUrl
                };
            };

            options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
        });
    }
}