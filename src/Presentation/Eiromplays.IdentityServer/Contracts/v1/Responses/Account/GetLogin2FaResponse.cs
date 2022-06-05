namespace Eiromplays.IdentityServer.Contracts.v1.Responses.Account;

public class GetLogin2FaResponse
{
    public string ReturnUrl { get; set; } = default!;
    
    public bool RememberMe { get; set; }
}