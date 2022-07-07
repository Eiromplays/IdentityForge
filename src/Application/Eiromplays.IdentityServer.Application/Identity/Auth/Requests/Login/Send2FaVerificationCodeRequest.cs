namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class Send2FaVerificationCodeRequest
{
    public string Provider { get; set; } = default!;

    public string ReturnUrl { get; set; } = default!;

    public bool RememberMe { get; set; }
}