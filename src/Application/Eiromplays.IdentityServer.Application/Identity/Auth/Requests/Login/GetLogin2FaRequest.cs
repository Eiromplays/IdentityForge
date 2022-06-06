namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class GetLogin2FaRequest
{
    [QueryParam] public string ReturnUrl { get; set; } = default!;
    
    [QueryParam] public bool RememberMe { get; set; }
}