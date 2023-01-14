namespace Eiromplays.IdentityServer.Application.Identity.Users.Password;

public class ChangePasswordRequest
{
    public string UserId { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;
}

public class ChangePasswordRequestValidator : Validator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator(IStringLocalizer<ChangePasswordRequestValidator> T)
    {
        RuleFor(p => p.Password)
            .NotEmpty()
            .UnlessAsync(async (changePasswordRequest, _, _) =>
            {
                var userService = Resolve<IUserService>();
                return !await userService.HasPasswordAsync(changePasswordRequest.UserId);
            });

        RuleFor(p => p.NewPassword)
            .NotEmpty();

        RuleFor(p => p.ConfirmNewPassword)
            .Equal(p => p.NewPassword)
                .WithMessage(T["Passwords do not match."]);
    }
}