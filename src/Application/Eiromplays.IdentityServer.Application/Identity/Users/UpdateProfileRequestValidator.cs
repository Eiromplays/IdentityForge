using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateProfileRequestValidator : Validator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator(IOptions<IdentityOptions> identityOptions)
    {
        var t = Resolve<IStringLocalizer<UpdateProfileRequestValidator>>();
        var fileUploadRequestValidator = Resolve<IValidator<FileUploadRequest?>>();

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
            .WithMessage(t["Invalid Email Address."])
            .MustAsync(async (updateUserRequest, email, _) =>
            {
                var userService = Resolve<IUserService>();

                return !await userService.ExistsWithEmailAsync(email, updateUserRequest.Id);
            })
            .WithMessage((_, email) => t["Email {0} is already registered.", email])
            .When((req, _) =>
                !string.IsNullOrWhiteSpace(req.Email) || identityOptions.Value.SignIn.RequireConfirmedEmail);

        RuleFor(p => p.Image)
            .SetValidator(fileUploadRequestValidator);

        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (updateUserRequest, phone, _) =>
            {
                var userService = Resolve<IUserService>();
                return !await userService.ExistsWithPhoneNumberAsync(phone!, updateUserRequest.Id);
            })
            .WithMessage((_, phone) => t["Phone number {0} is already registered.", phone!])
            .Must(phoneNumber => phoneNumberUtil.IsValidNumber(phoneNumberUtil.Parse(phoneNumber, null)))
            .WithMessage(t["Invalid Phone Number."])
            .Unless(u =>
                string.IsNullOrWhiteSpace(u.PhoneNumber) && !identityOptions.Value.SignIn.RequireConfirmedPhoneNumber)
            .NotEmpty()
            .When(u => !string.IsNullOrWhiteSpace(u.PhoneNumber) || identityOptions.Value.SignIn.RequireConfirmedPhoneNumber);
    }
}