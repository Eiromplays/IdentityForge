namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

public class GetLogin2FaResponse
{
    public string ReturnUrl { get; set; } = default!;
    
    public bool RememberMe { get; set; }
}