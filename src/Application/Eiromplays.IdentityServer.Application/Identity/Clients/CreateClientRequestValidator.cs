using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class CreateClientRequestValidator : Validator<CreateClientRequest>
{
    public CreateClientRequestValidator(IClientService clientService, IStringLocalizer<CreateClientRequestValidator> T)
    {
        RuleFor(p => p.ClientId)
            .NotEmpty()
            .MustAsync(async (clientId, _) => !await clientService.ExistsWithClientIdAsync(clientId))
            .WithMessage((_, clientId) => string.Format(T["Client {0} is already registered."], clientId));

        RuleFor(p => p.ClientName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(p => p.ClientUri)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.ClientUri));

        RuleFor(p => p.LogoUri)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.LogoUri));

        RuleFor(c => c.ProtocolType)
            .Must(protocol => protocol is IdentityServerConstants.ProtocolTypes.Saml2p
                or IdentityServerConstants.ProtocolTypes.OpenIdConnect
                or IdentityServerConstants.ProtocolTypes.WsFederation)
            .WithMessage((_, protocol) => string.Format(T["Protocol {0} is not supported."], protocol));

        RuleFor(request => request.AllowedGrantTypes)
            .Must(grantTypes => grantTypes.Exists(grantType => GrantTypes.Implicit.Contains(grantType) ||
                                                               GrantTypes.ImplicitAndClientCredentials.Contains(
                                                                   grantType) || GrantTypes.Code.Contains(grantType) ||
                                                               GrantTypes.CodeAndClientCredentials
                                                                   .Contains(grantType) ||
                                                               GrantTypes.Hybrid.Contains(grantType) ||
                                                               GrantTypes.HybridAndClientCredentials
                                                                   .Contains(grantType) ||
                                                               GrantTypes.ClientCredentials.Contains(grantType) ||
                                                               GrantTypes.ResourceOwnerPassword.Contains(grantType) ||
                                                               GrantTypes.ResourceOwnerPasswordAndClientCredentials.Contains(grantType) ||
                                                               GrantTypes.DeviceFlow.Contains(grantType) ||
                                                               GrantTypes.Ciba.Contains(grantType)))
            .WithMessage((_, grantTypes) => string.Format(T["Grant types {0} are not valid."], grantTypes));
    }
}