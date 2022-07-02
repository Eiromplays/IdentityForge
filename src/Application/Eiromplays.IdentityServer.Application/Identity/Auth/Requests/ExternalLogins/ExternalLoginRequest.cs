namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;

public class ExternalLoginRequest
{
    [QueryParam]
    public string Provider { get; set; } = default!;

    [QueryParam]
    public string ReturnUrl { get; set; } = default!;
}