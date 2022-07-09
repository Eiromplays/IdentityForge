namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;

public class GetEnableAuthenticatorResponse
{
    public IList<string> ValidProviders { get; set; } = new List<string>();

    public string? SharedKey { get; set; }

    public string? AuthenticatorUri { get; set; }
}