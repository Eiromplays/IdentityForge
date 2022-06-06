namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class Login2FaRequest
{
    public string TwoFactorCode { get; set; } = default!;

    public bool RememberMachine { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = default!;

    public string Error { get; set; } = default!;
}