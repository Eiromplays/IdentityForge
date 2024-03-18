namespace IdentityForge.IdentityServer.Configurations;

public class SwaggerSettings
{
    public const string SectionName = "Swagger";

    public bool Enable { get; set; } = true;

    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }

    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactUrl { get; set; }

    public bool License { get; set; }
    public string? LicenseName { get; set; }
    public string? LicenseUrl { get; set; }
}

public static class SwaggerSettingsExtensions
{
    public static SwaggerSettings GetSwaggerSettings(this IConfiguration configuration)
        => configuration.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>() ?? new SwaggerSettings();
}