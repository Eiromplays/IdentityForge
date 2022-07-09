using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class Login2FaRequest
{
    public string Provider { get; set; } = nameof(TwoFactorAuthenticationProviders.App);

    public string TwoFactorCode { get; set; } = default!;

    public bool RememberMachine { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = default!;

    public string Error { get; set; } = default!;
}

public class Login2FaRequestValidator : Validator<Login2FaRequest>
{
    public Login2FaRequestValidator(IStringLocalizer<Login2FaRequestValidator> T)
    {
        RuleFor(u => u.Provider).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(T["The {0} field is required.", nameof(Login2FaRequest.Provider)])
            .Must((provider) => Enum.TryParse<TwoFactorAuthenticationProviders>(provider, true, out _))
            .WithMessage(T["The {0} field is invalid.", nameof(Login2FaRequest.Provider)]);

        RuleFor(x => x.TwoFactorCode)
            .NotEmpty()
            .Length(6, 7)
            .WithMessage("The {0} must be at least {2} and at max {1} characters long.");
    }
}