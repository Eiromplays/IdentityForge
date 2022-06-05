namespace Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

public class GetLogoutRequest
{
    [QueryParam] public string LogoutId { get; set; } = default!;
}