namespace Eiromplays.IdentityServer.Application.Identity.Users.Password;

public class SetPasswordRequest
{
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;

    public SetPasswordRequest(string password, string confirmPassword)
    {
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public SetPasswordRequest()
    {
    }
}

public class SetPasswordRequestValidator : Validator<SetPasswordRequest>
{
    public SetPasswordRequestValidator(IStringLocalizer<ChangePasswordRequestValidator> T)
    {
        RuleFor(p => p.Password)
            .NotEmpty();

        RuleFor(p => p.ConfirmPassword)
            .Equal(p => p.Password)
                .WithMessage(T["Passwords do not match."]);
    }
}