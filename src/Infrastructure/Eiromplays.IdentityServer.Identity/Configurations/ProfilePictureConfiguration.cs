namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class ProfilePictureConfiguration
{
    public bool IsProfilePictureEnabled {  get; set; }

    public bool AutoGenerateProfilePicture { get; set; }

    public string? ProfilePictureGeneratorUrl { get; set; } = "https://avatars.dicebear.com/api/initials/";
}