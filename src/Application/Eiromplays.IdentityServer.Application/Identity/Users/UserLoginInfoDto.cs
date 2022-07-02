namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UserLoginInfoDto
{
    public string LoginProvider { get; set; } = default!;

    public string ProviderKey { get; set; } = default!;

    public string ProviderDisplayName { get; set; } = default!;
}