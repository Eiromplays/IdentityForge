namespace Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

public class GetLoginRequest
{
    [QueryParam] public string ReturnUrl { get; set; } = default!;
}