namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class ConfirmEmailRequest
{
    [QueryParam]
    public string UserId { get; set; } = default!;

    [QueryParam]
    public string Code { get; set; } = default!;

    [QueryParam]
    public string ReturnUrl { get; set; } = default!;
}