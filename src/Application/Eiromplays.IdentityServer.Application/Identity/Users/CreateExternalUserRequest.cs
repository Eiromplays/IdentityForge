namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class CreateExternalUserRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
}