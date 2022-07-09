namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateProfileRequest
{
    public string Id { get; set; } = default!;

    public string? DisplayName { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = default!;

    public string? GravatarEmail { get; set; }

    public FileUploadRequest? Image { get; set; }

    public bool DeleteCurrentImage { get; set; } = false;

    public bool RevokeUserSessions { get; set; } = true;
}