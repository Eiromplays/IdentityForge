namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UpdateUserRequestValidator : Validator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(IStringLocalizer<UpdateUserRequestValidator> T)
    {
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
                .WithMessage(T["Invalid Email Address."])
            .MustAsync(async (user, email, _) =>
            {
                var userService = Resolve<IUserService>();
                return !await userService.ExistsWithEmailAsync(email, user.Id);
            })
                .WithMessage((_, email) => string.Format(T["Email {0} is already registered."], email));

        RuleFor(p => p.Image)
            .SetValidator(fileUploadRequestValidator);

        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (user, phone, _) =>
            {
                var userService = Resolve<IUserService>();
                return !await userService.ExistsWithPhoneNumberAsync(phone!, user.Id);
            })
                .WithMessage((_, phone) => string.Format(T["Phone number {0} is already registered."], phone))
            .Must(phoneNumber => phoneNumberUtil.IsValidNumber(phoneNumberUtil.Parse(phoneNumber, null)))
                .WithMessage(T["Invalid Phone Number."])
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
    }
}