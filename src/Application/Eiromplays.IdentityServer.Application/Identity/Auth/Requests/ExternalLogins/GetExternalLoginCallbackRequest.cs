namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;

public class GetExternalLoginCallbackRequest
{
    public string ReturnUrl { get; set; } = default!;

    [QueryParam] public string RemoteError { get; set; } = default!;
}