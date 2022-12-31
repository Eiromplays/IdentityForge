using Eiromplays.IdentityServer.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class CreateUserRequestValidator : Validator<CreateUserRequest>
{
    public CreateUserRequestValidator(IUserService userService, IOptions<IdentityOptions> identityOptions, IStringLocalizer<CreateUserRequestValidator> T)
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
            .When((req, _) => (req.Provider is nameof(AccountProviders.Email) or nameof(AccountProviders.External) &&
                              !string.IsNullOrWhiteSpace(req.Email)) || identityOptions.Value.SignIn.RequireConfirmedEmail);

        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MustAsync(async (name, _) => !await userService.ExistsWithNameAsync(name))
                .WithMessage((_, name) => T["Username {0} is already taken.", name]);

        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!))
                .WithMessage((_, phone) => T["Phone number {0} is already registered.", phone!])
                .Unless(u =>
                string.IsNullOrWhiteSpace(u.PhoneNumber) && u.Provider != nameof(AccountProviders.Phone) &&
                !identityOptions.Value.SignIn.RequireConfirmedPhoneNumber)
            .Must(phoneNumber => phoneNumberUtil.IsValidNumber(phoneNumberUtil.Parse(phoneNumber, null)))
            .NotEmpty()
            .When(u => (!string.IsNullOrWhiteSpace(u.PhoneNumber) && u.Provider == nameof(AccountProviders.Phone)) || identityOptions.Value.SignIn.RequireConfirmedPhoneNumber);

        RuleFor(p => p.FirstName).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.LastName).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .When(u => u.Provider == nameof(AccountProviders.Email) || !string.IsNullOrWhiteSpace(u.Password));

        RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Equal(p => p.Password)
            .When(u => u.Provider == nameof(AccountProviders.Email) || !string.IsNullOrWhiteSpace(u.ConfirmPassword));

        RuleFor(p => p.Agreement)
            .Equal(true)
            .WithMessage(T["You must agree to the terms and conditions."]);
    }
}