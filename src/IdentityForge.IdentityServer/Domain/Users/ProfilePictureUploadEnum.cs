using Ardalis.SmartEnum;

namespace IdentityForge.IdentityServer.Domain.Users;

public abstract class ProfilePictureUploadEnum : SmartEnum<ProfilePictureUploadEnum>
{
    public static readonly ProfilePictureUploadEnum File = new FileType();
    public static readonly ProfilePictureUploadEnum Disabled = new DisabledType();

    protected ProfilePictureUploadEnum(string name, int value) : base(name, value)
    {
    }

    private class FileType : ProfilePictureUploadEnum
    {
        public FileType() : base("File", 0) { }
    }

    private class DisabledType : ProfilePictureUploadEnum
    {
        public DisabledType() : base("Disabled", 1) { }
    }
}