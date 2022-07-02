namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;

public class TwoFactorAuthenticationResponse
{
    public bool HasAuthenticator { get; set; }

    public int RecoveryCodesLeft { get; set; }

    public bool Is2FaEnabled { get; set; }

    public bool IsMachineRemembered { get; set; }
}