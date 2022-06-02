namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class AccountConfiguration
{
    public RegisterConfiguration RegisterConfiguration { get; set; } = null!;

    public LoginConfiguration LoginConfiguration { get; set; } = null!;

    public ProfilePictureConfiguration ProfilePictureConfiguration { get; set; } = null!;
}