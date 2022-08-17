using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class CreateClientRequest
{
    public string ClientType { get; set; } = nameof(ClientTypes.Spa);

    public string ClientId { get; set; } = default!;

    public string ClientName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string ClientUri { get; set; } = default!;

    public string LogoUri { get; set; } = default!;

    public bool RequireConsent { get; set; } = false;

    public List<string> RedirectUris { get; set; } = new();

    public List<string> PostLogoutRedirectUris { get; set; } = new();
}