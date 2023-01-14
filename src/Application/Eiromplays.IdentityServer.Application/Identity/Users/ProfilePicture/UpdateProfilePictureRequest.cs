namespace Eiromplays.IdentityServer.Application.Identity.Users.ProfilePicture;

public class UpdateProfilePictureRequest
{
    public UpdateProfilePictureRequest(){}

    public UpdateProfilePictureRequest(string? oldProfilePicturePath, FileUploadRequest? image)
    {
        OldProfilePicturePath = oldProfilePicturePath;
        Image = image;
    }

    public string? OldProfilePicturePath { get; set; }

    public FileUploadRequest? Image { get; set; }
}