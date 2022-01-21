namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class AccountConfiguration
{
    public RegisterConfiguration? RegisterConfiguration { get; set; }

    public LoginConfiguration? LoginConfiguration { get; set; }

    public ProfilePictureConfiguration? ProfilePictureConfiguration { get; set; }
}