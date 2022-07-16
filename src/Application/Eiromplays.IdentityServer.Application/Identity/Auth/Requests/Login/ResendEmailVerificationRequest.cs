namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class ResendEmailVerificationRequest
{
    public string Email { get; set; } = default!;
}

public class ResendEmailVerificationRequestValidator : Validator<ResendEmailVerificationRequest>
{
    public ResendEmailVerificationRequestValidator(IStringLocalizer<ResendEmailVerificationRequestValidator> T)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(T["Email is required"]);
    }
}