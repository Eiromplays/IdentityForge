namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class ConfirmPhoneNumberRequest
{
    [QueryParam] public string UserId { get; set; } = default!;

    [QueryParam] public string Code { get; set; } = default!;
}