namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;

public class EnableAuthenticatorResponse
{
    public List<string> RecoveryCodes { get; set; } = new();
}