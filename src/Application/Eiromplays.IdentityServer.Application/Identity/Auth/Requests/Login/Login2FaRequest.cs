namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class Login2FaRequest
{
    public string TwoFactorCode { get; set; } = default!;

    public bool RememberMachine { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = default!;

    public string Error { get; set; } = default!;
}

public class Login2FaRequestValidator : Validator<Login2FaRequest>
{
    public Login2FaRequestValidator()
    {
        RuleFor(x => x.TwoFactorCode)
            .NotEmpty()
            .Length(6, 7)
            .WithMessage("The {0} must be at least {2} and at max {1} characters long.");
    }
}