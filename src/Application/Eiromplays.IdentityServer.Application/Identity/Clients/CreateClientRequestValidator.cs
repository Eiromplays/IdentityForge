using System.Text.Json;
using Eiromplays.IdentityServer.Domain.Enums;

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

        RuleFor(request => request.ClientType)
            .Must(clientType => Enum.TryParse<ClientTypes>(clientType, out _))
            .WithMessage((_, clientType) => string.Format(T["Client type {0} is not valid."], clientType));

        RuleFor(req => req.RedirectUris)
            .Must(redirectUrls => redirectUrls.All(redirectUrl =>
                Uri.TryCreate(redirectUrl, UriKind.Absolute, out _)))
            .WithMessage(req => T[$"Redirect uris {JsonSerializer.Serialize(req.RedirectUris.All(redirectUrl => !Uri.TryCreate(redirectUrl, UriKind.Absolute, out _)))} are not valid."]);

        RuleFor(req => req.PostLogoutRedirectUris)
            .Must(postLogoutRedirectUris => postLogoutRedirectUris.All(redirectUrl =>
                Uri.TryCreate(redirectUrl, UriKind.Absolute, out _)))
            .WithMessage(req => T[$"Post logout redirect uris {JsonSerializer.Serialize(req.PostLogoutRedirectUris.All(postLogoutRedirectUris => !Uri.TryCreate(postLogoutRedirectUris, UriKind.Absolute, out _)))} are not valid."]);
    }
}