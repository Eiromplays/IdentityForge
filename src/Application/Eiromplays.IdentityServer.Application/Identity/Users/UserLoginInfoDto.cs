namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UserLoginInfoDto
{
    public string? LoginProvider { get; set; }
    
    public string? ProviderKey { get; set; }
    
    public string? ProviderDisplayName { get; set; }
}