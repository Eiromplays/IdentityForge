namespace Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

public class UpdateIdentityProviderRequestValidator : Validator<UpdateIdentityProviderRequest>
{
    public UpdateIdentityProviderRequestValidator()
    {
        RuleFor(p => p.Scheme)
            .NotEmpty();

        RuleFor(p => p.DisplayName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(p => p.Type)
            .NotEmpty()
            .MaximumLength(200);
    }
}