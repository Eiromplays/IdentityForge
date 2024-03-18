using IdentityForge.IdentityServer.Domain.Users;

namespace IdentityForge.IdentityServer.Configurations;

public sealed class ProfilePictureConfiguration
{
    public const string SectionName = "ProfilePicture";

    public bool Enabled { get; set; } = true;

    public bool AutoGenerate { get; set; } = true;

    public ProfilePictureUploadEnum ProfilePictureUploadType { get; set; } = ProfilePictureUploadEnum.File;

    public IReadOnlyCollection<string> AllowedFileExtensions { get; set; } = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public string? BaseUrl { get; set; }

    public string DefaultUrl { get; set; } = "https://avatars.dicebear.com/api/initials/";
}