namespace Eiromplays.IdentityServer.Application.Identity.IdentityResources;

public class CreateIdentityResourceRequestValidator : Validator<CreateIdentityResourceRequest>
{
    public CreateIdentityResourceRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(p => p.DisplayName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(75);
    }
}