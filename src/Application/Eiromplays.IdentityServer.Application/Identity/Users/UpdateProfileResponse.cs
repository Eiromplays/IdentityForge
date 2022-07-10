namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateProfileResponse
{
    public string? Message { get; set; }

    public bool LogoutRequired { get; set; }
    public string ReturnUrl { get; set; } = default!;
}