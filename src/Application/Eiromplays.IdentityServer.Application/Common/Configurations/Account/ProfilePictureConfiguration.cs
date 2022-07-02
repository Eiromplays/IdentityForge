using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class ProfilePictureConfiguration
{
    public bool Enabled { get; set; } = true;

    public bool AutoGenerate { get; set; } = true;

    public ProfilePictureUploadType ProfilePictureUploadType { get; set; } = ProfilePictureUploadType.File;

    public List<string> AllowedFileExtensions { get; set; } = new() { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public string? BaseUrl { get; set; }

    public string? DefaultUrl { get; set; } = "https://avatars.dicebear.com/api/initials/";
}