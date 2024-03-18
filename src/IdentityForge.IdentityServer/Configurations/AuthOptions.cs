namespace IdentityForge.IdentityServer.Configurations;

public class AuthOptions
{
    public const string SectionName = "Auth";

    public string Audience { get; set; } = default!;
    public string Authority { get; set; } = default!;
    public string AuthorizationUrl { get; set; } = default!;
    public string TokenUrl { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;

    public List<ExternalProvider> ExternalProviders { get; set; } = new();
}

public sealed class ExternalProvider
{
    public bool IsEnabled { get; set; }

    public string Name { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;

    public string CallbackPath { get; set; } = default!;
}

public static class AuthOptionsExtensions
{
    public static AuthOptions GetAuthOptions(this IConfiguration configuration)
        => configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>() ?? new AuthOptions();
}