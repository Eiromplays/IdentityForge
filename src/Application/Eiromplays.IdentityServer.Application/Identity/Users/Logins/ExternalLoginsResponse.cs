using Microsoft.AspNetCore.Authentication;

namespace Eiromplays.IdentityServer.Application.Identity.Users.Logins;

public class ExternalLoginsResponse
{
    public List<UserLoginInfoDto> CurrentLogins { get; set; } = new();
    
    public List<AuthenticationScheme> OtherLogins { get; set; } = new();
    
    public bool ShowRemoveButton { get; set; }
}