namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class CreateClientRequestValidator : CustomValidator<CreateClientRequest>
{
    public CreateClientRequestValidator(IClientService clientService, IStringLocalizer<CreateClientRequestValidator> T)
    {
        RuleFor(p => p.ClientId)
            .NotEmpty()
            .MustAsync(async (clientId, _) => !await clientService.ExistsWithClientIdAsync(clientId))
            .WithMessage((_, clientId) => string.Format(T["Client {0} is already registered."], clientId));
        
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