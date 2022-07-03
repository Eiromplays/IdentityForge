namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class ConfirmEmailResponse
{
    public bool PhoneNumberVerificationSent { get; set; }

    public string Message { get; set; } = default!;

    public string ReturnUrl { get; set; } = default!;
}