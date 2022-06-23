namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class GetLoginRequest
{
    [QueryParam] public string ReturnUrl { get; set; } = default!;
}