using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

namespace Eiromplays.IdentityServer.Validation.v1.Account;

public class Login2FaRequestValidator : Validator<Login2FaRequest>
{
    public Login2FaRequestValidator()
    {
        RuleFor(x => x.TwoFactorCode)
            .NotEmpty()
            .Length(6, 7)
            .WithMessage("The {0} must be at least {2} and at max {1} characters long.");
    }
}