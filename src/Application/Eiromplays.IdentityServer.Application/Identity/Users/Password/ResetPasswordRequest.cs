namespace Eiromplays.IdentityServer.Application.Identity.Users.Password;

public class ResetPasswordRequest
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Token { get; set; } = default!;
}