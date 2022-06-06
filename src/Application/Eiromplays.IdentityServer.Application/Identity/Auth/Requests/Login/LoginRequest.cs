namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class LoginRequest
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}