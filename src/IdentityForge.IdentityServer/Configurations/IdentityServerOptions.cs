using Duende.IdentityServer.Configuration;

namespace IdentityForge.IdentityServer.Configurations;

public static class IdentityServerOptionsExtensions
{
    public static IdentityServerOptions GetIdentityServerOptions(this IConfiguration configuration)
        => configuration.GetSection(nameof(IdentityServerOptions)).Get<IdentityServerOptions>() ?? new IdentityServerOptions();
}