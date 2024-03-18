using Microsoft.AspNetCore.Identity;

namespace IdentityForge.IdentityServer.Configurations;

public static class IdentityOptionsExtensions
{
    public static IdentityOptions GetIdentityOptions(this IConfiguration configuration)
        => configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>() ?? new IdentityOptions();

    public static IConfigurationSection GetIdentityOptionsSection(this IConfiguration configuration)
        => configuration.GetSection(nameof(IdentityOptions));
}