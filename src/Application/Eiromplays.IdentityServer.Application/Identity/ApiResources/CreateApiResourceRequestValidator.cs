namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class CreateApiResourceRequestRequestValidator : CustomValidator<CreateApiResourceRequest>
{
    public CreateApiResourceRequestRequestValidator()
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