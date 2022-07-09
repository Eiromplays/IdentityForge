namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class SendSmsLoginCodeRequest
{
    public string PhoneNumber { get; set; } = default!;
}

public class SendSmsLoginCodeRequestValidator : Validator<SendSmsLoginCodeRequest>
{
    public SendSmsLoginCodeRequestValidator(IStringLocalizer<SendSmsLoginCodeRequestValidator> T)
    {
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(T["PhoneNumber is required"])
            .Must(p => phoneNumberUtil.IsValidNumber(phoneNumberUtil.Parse(p, null)))
            .WithMessage(T["PhoneNumber is invalid"]);
    }
}