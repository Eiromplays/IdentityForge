namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public class CreateApiScopeRequestValidator : CustomValidator<CreateApiScopeRequest>
{
    public CreateApiScopeRequestValidator()
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