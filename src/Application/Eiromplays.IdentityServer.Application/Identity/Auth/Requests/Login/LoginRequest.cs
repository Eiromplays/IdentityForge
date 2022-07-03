using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class LoginRequest
{
    public string Provider { get; set; } = AccountProviders.Email.ToString();

    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;

    public string Code { get; set; } = default!;

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator(IStringLocalizer<LoginRequestValidator> T)
    {
        RuleFor(u => u.Provider).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(T["The {0} field is required.", nameof(AccountProviders)])
            .Must(provider => Enum.TryParse<AccountProviders>(provider, true, out _))
            .WithMessage(T["The {0} field is invalid.", nameof(AccountProviders)]);

        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(100)
            .When(u => u.Provider == AccountProviders.Email.ToString() || !string.IsNullOrWhiteSpace(u.Password));

        RuleFor(x => x.ReturnUrl)
            .MaximumLength(2000);

        RuleFor(x => x.Code)
            .MaximumLength(100)
            .When(u => u.Provider == AccountProviders.Phone.ToString());
    }
}