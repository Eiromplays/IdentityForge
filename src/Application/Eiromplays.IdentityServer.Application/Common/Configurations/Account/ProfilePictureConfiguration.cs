namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class ProfilePictureConfiguration
{
    public bool Enabled { get; set; } = true;

    public bool AutoGenerate { get; set; } = true;

    public bool AllowUploading { get; set; } = true;

    public string? DefaultUrl { get; set; } = "https://avatars.dicebear.com/api/initials/";
}