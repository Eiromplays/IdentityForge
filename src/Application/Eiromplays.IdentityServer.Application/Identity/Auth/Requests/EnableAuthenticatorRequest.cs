using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests;

public class EnableAuthenticatorRequest
{
    // Provider defaults to App Authenticator if not specified
    public string Provider { get; set; } = nameof(TwoFactorAuthenticationProviders.App);
    public string Code { get; set; } = default!;
}

public class EnableAuthenticatorRequestValidator : Validator<EnableAuthenticatorRequest>
{
    public EnableAuthenticatorRequestValidator(IUserService userService, ICurrentUser currentUser, IStringLocalizer<EnableAuthenticatorRequestValidator> T)
    {
        RuleFor(u => u.Provider).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(T["The {0} field is required.", nameof(EnableAuthenticatorRequest.Provider)])
            .MustAsync(async (provider, __) => Enum.TryParse<TwoFactorAuthenticationProviders>(provider, true, out _) &&
                                           (await userService.GetValidTwoFactorProvidersAsync(currentUser.GetUserId()))
                                           .Contains(provider, StringComparer.OrdinalIgnoreCase))
            .WithMessage(T["The {0} field is invalid.", nameof(EnableAuthenticatorRequest.Provider)]);

        RuleFor(x => x.Code).NotEmpty().WithMessage("Verification code is required.").Length(6, 7)
            .WithMessage("Verification code must be at least 6 and at max 7 characters long.")
            .WithName("Verification Code")
            .UnlessAsync(async (req, _) =>
                (await userService.GetValidTwoFactorProvidersAsync(currentUser.GetUserId())).Contains(
                    req.Provider, StringComparer.OrdinalIgnoreCase));
    }
}