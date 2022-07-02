namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;

public class EnableAuthenticator
{
    public string Code { get; set; } = default!;

    public string? SharedKey { get; set; }

    public string? AuthenticatorUri { get; set; }
}

public class EnableAuthenticatorValidator : Validator<EnableAuthenticator>
{
    public EnableAuthenticatorValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Verification code is required.").Length(6, 7)
            .WithMessage("Verification code must be at least 6 and at max 7 characters long.")
            .WithName("Verification Code");
    }
}