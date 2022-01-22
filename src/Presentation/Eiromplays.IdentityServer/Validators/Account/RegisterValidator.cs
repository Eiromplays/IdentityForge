using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.ViewModels.Account;
using FluentValidation;

namespace Eiromplays.IdentityServer.Validators.Account;

public class RegisterValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterValidator(AccountConfiguration accountConfiguration)
    {
        When(_ => accountConfiguration.LoginConfiguration is not {LoginPolicy: LoginPolicy.Email}, () =>
        {
            RuleFor(x => x.UserName)
                .NotEmpty();

            RuleFor(x => x.DisplayName)
                .NotEmpty();
        });

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .NotNull()
            .Equal(x => x.Password);
    }
}