namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class UpdateClientRequestValidator : CustomValidator<UpdateClientRequest>
{
    public UpdateClientRequestValidator(IStringLocalizer<UpdateClientRequestValidator> T)
    {
        RuleFor(p => p.Id)
            .NotEmpty();

        RuleFor(p => p.ClientName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(75);
    }
}