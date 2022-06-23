namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class UpdateApiResourceRequestValidator : Validator<UpdateApiResourceRequest>
{
    public UpdateApiResourceRequestValidator(IStringLocalizer<UpdateApiResourceRequestValidator> T)
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