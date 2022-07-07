namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateUserRequest
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

    public bool EmailConfirmed { get; set; } = false;
    public bool PhoneNumberConfirmed { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public bool LockoutEnabled { get; set; } = false;
    public bool IsActive { get; set; } = false;
}