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
        
        RuleFor(p => p.ClientUri)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.ClientUri));
        
        RuleFor(p => p.LogoUri)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.LogoUri));
    }
}