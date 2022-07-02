namespace Eiromplays.IdentityServer.Application.Identity.Users.Logins;

public class ExternalLoginConfirmationResponse
{
    public string Message { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;
}