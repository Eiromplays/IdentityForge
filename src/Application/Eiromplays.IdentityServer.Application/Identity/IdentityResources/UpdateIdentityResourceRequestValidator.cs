namespace Eiromplays.IdentityServer.Application.Identity.IdentityResources;

public class UpdateIdentityResourceRequestValidator : Validator<UpdateIdentityResourceRequest>
{
    public UpdateIdentityResourceRequestValidator(IStringLocalizer<UpdateIdentityResourceRequestValidator> T)
    {
        RuleFor(p => p.Id)
            .NotEmpty();

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