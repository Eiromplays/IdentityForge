using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class CreateClientRequest
{
    public bool Enabled { get; set; } = true;

    public string ClientId { get; set; } = default!;

    public string ProtocolType { get; set; } = IdentityServerConstants.ProtocolTypes.OpenIdConnect;

    public bool RequireClientSecret { get; set; } = true;

    public string ClientName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string ClientUri { get; set; } = default!;

    public string LogoUri { get; set; } = default!;

    public bool RequireConsent { get; set; } = false;

    public bool AllowRememberConsent { get; set; } = true;

    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    public List<string> AllowedGrantTypes { get; set; } = GrantTypes.Code.ToList();

    public bool RequirePkce { get; set; } = true;
    public bool AllowPlainTextPkce { get; set; }
    public bool RequireRequestObject { get; set; }
    public bool AllowAccessTokensViaBrowser { get; set; }
}