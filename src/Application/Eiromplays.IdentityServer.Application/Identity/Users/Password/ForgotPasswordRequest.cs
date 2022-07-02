namespace Eiromplays.IdentityServer.Application.Identity.Users.Password;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = default!;
}

public class ForgotPasswordRequestValidator : Validator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> T) =>
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."]);
}