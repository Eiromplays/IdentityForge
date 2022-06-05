namespace Eiromplays.IdentityServer.Contracts.v1.Responses.Account;

public class GetLogoutResponse
{
    public string LogoutId { get; set; } = default!;
    
    public bool ShowLogoutPrompt { get; set; } = true;
}