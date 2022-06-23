namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public class UpdateApiScopeRequestValidator : Validator<UpdateApiScopeRequest>
{
    public UpdateApiScopeRequestValidator(IStringLocalizer<UpdateApiScopeRequestValidator> T)
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