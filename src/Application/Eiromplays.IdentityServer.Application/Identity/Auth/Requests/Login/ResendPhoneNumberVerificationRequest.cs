namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class ResendPhoneNumberVerificationRequest
{
    public string PhoneNumber { get; set; } = default!;
}

public class ResendPhoneNumberVerificationRequestValidator : Validator<ResendPhoneNumberVerificationRequest>
{
    public ResendPhoneNumberVerificationRequestValidator(IStringLocalizer<ResendPhoneNumberVerificationRequestValidator> T)
    {
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(T["PhoneNumber is required"])
            .Must(p => phoneNumberUtil.IsValidNumber(phoneNumberUtil.Parse(p, null)))
            .WithMessage(T["PhoneNumber is invalid"]);
    }
}