namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class ProfilePictureConfiguration
{
    public bool IsProfilePictureEnabled { get; set; } = true;

    public bool AutoGenerateProfilePicture { get; set; } = true;

    public string? ProfilePictureGeneratorUrl { get; set; } = "https://avatars.dicebear.com/api/initials/";
}