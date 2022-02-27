namespace Eiromplays.IdentityServer.Application.DTOs.User;

public class UserLoginDto
{
    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? ProviderKey { get; set; }

    public string? LoginProvider { get; set; }

    public string? ProviderDisplayName { get; set; }
}