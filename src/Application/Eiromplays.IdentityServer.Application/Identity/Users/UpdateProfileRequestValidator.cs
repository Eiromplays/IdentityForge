using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateProfileRequestValidator : Validator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator(IUserService userService, IOptions<IdentityOptions> identityOptions, IStringLocalizer<UpdateProfileRequestValidator> T, IValidator<FileUploadRequest?> fileUploadRequestValidator)
    {
        RuleFor(p => p.Id)
            .NotEmpty();

        RuleFor(p => p.FirstName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.LastName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage(T["Invalid Email Address."])
            .MustAsync(async (updateUserRequest, email, _) =>
                !await userService.ExistsWithEmailAsync(email, updateUserRequest.Id))
            .WithMessage((_, email) => T["Email {0} is already registered.", email])
            .When((req, _) =>
                !string.IsNullOrWhiteSpace(req.Email) || identityOptions.Value.SignIn.RequireConfirmedEmail);

        RuleFor(p => p.Image)
            .SetValidator(fileUploadRequestValidator);

        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (updateUserRequest, phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!, updateUserRequest.Id))
            .WithMessage((_, phone) => T["Phone number {0} is already registered.", phone!])
            .Must(phoneNumber => phoneNumberUtil.IsValidNumber(phoneNumberUtil.Parse(phoneNumber, null)))
            .WithMessage(T["Invalid Phone Number."])
            .Unless(u =>
                string.IsNullOrWhiteSpace(u.PhoneNumber) && !identityOptions.Value.SignIn.RequireConfirmedPhoneNumber)
            .NotEmpty()
            .When(u => !string.IsNullOrWhiteSpace(u.PhoneNumber) || identityOptions.Value.SignIn.RequireConfirmedPhoneNumber);
    }
}