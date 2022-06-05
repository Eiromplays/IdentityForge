namespace Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

public class GetLogin2FaRequest
{
    [QueryParam] public string ReturnUrl { get; set; } = default!;
    
    [QueryParam] public bool RememberMe { get; set; }
}