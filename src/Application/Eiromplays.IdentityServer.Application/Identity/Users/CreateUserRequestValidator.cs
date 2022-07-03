using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class CreateUserRequestValidator : Validator<CreateUserRequest>
{
    public CreateUserRequestValidator(IUserService userService, IStringLocalizer<CreateUserRequestValidator> T)
    {
        RuleFor(u => u.Provider).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(T["The {0} field is required.", nameof(CreateUserRequest.Provider)])
            .Must(provider => Enum.TryParse<AccountProviders>(provider, true, out _))
            .WithMessage(T["The {0} field is invalid.", nameof(CreateUserRequest.Provider)]);

        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."])
            .MustAsync(async (email, _) =>
                !await userService.ExistsWithEmailAsync(email))
                .WithMessage((_, email) => T["Email {0} is already registered.", email])
            .When((req, _) => (req.Provider == AccountProviders.Email.ToString() || req.Provider == AccountProviders.External.ToString()) &&
                              !string.IsNullOrWhiteSpace(req.Email));

        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MustAsync(async (name, _) => !await userService.ExistsWithNameAsync(name))
                .WithMessage((_, name) => T["Username {0} is already taken.", name]);

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!))
                .WithMessage((_, phone) => T["Phone number {0} is already registered.", phone!])
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber) && u.Provider != AccountProviders.Phone.ToString());

        RuleFor(p => p.FirstName).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.LastName).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .When(u => u.Provider == AccountProviders.Email.ToString() || !string.IsNullOrWhiteSpace(u.Password));

        RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Equal(p => p.Password)
            .When(u => u.Provider == AccountProviders.Email.ToString() || !string.IsNullOrWhiteSpace(u.ConfirmPassword));
    }
}