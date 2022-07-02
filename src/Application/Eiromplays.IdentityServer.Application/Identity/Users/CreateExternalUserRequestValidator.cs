namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class CreateExternalUserRequestValidator : Validator<CreateExternalUserRequest>
{
    // TODO: FIX using GetAwaiter().GetResult() to wait for async calls
    // Related issue: https://github.com/fullstackhero/dotnet-webapi-boilerplate/issues/639#issuecomment-1118324086
    // This is a workaround for the issue above.
    // Will be fixed once the IdentityServer project uses FastEndpoints, instead of Controllers.

    public CreateExternalUserRequestValidator(IUserService userService, IStringLocalizer<CreateExternalUserRequestValidator> T)
    {
        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."])
            .Must((_, email, _) => !userService.ExistsWithEmailAsync(email).GetAwaiter().GetResult())
                .WithMessage((_, email) => T["Email {0} is already registered.", email]);

        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .Must((_, name, _) => !userService.ExistsWithNameAsync(name).GetAwaiter().GetResult())
                .WithMessage((_, name) => T["Username {0} is already taken.", name]);

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .Must((_, phone, _) => !userService.ExistsWithPhoneNumberAsync(phone!).GetAwaiter().GetResult())
                .WithMessage((_, phone) => T["Phone number {0} is already registered.", phone!])
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
    }
}