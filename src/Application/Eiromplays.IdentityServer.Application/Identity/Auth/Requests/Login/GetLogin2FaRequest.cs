namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class GetLogin2FaRequest
{
    public string ReturnUrl { get; set; } = default!;
    
    public bool RememberMe { get; set; }
}