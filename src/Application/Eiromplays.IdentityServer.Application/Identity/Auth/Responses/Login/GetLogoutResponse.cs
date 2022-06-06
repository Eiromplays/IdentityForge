namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

public class GetLogoutResponse
{
    public string LogoutId { get; set; } = default!;
    
    public bool ShowLogoutPrompt { get; set; } = true;
}