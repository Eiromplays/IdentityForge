namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;

public class LinkExternalLoginRequest
{
    public string Provider { get; set; } = default!;

    public string ReturnUrl { get; set; } = default!;
}