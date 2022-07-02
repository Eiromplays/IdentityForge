namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

public class LogoutResponse
{
    public string? PostLogoutRedirectUri { get; set; }
    public string? ClientName { get; set; }
    public string? SignOutIframeUrl { get; set; }

    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string? LogoutId { get; set; }
    public bool TriggerExternalSignout => ExternalAuthenticationScheme is not null;
    public string? ExternalAuthenticationScheme { get; set; }
}