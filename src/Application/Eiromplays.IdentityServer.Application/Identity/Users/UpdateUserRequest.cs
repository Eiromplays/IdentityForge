namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateUserRequest : UpdateProfileRequest
{
    public bool EmailConfirmed { get; set; } = false;
    public bool PhoneNumberConfirmed { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public bool LockoutEnabled { get; set; } = false;
    public bool IsActive { get; set; } = false;
}