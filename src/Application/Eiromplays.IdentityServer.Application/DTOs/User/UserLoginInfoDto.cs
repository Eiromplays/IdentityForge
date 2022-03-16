namespace Eiromplays.IdentityServer.Application.DTOs.User;

public class UserLoginInfoDto
{
    public UserLoginInfoDto(string? loginProvider, string? providerKey, string? displayName)
    {
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = displayName;
    }
    
    public string? LoginProvider { get; set; }
    
    public string? ProviderKey { get; set; }
    
    public string? ProviderDisplayName { get; set; }
}