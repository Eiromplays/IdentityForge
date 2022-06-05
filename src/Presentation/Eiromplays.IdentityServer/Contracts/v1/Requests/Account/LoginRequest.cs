namespace Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

public class LoginRequest
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}