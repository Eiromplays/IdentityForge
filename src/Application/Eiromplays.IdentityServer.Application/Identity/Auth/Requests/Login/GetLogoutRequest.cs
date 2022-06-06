namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class GetLogoutRequest
{
    [QueryParam] public string LogoutId { get; set; } = default!;
}