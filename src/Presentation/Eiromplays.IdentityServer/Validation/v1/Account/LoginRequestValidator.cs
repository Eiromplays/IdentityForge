using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

namespace Eiromplays.IdentityServer.Validation.v1.Account;

public class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ReturnUrl)
            .MaximumLength(2000);
    }
}