using Duende.IdentityServer.Models;

namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientDto
{
        public int Id { get; set; }
        
        public bool Enabled { get; set; } = true;

        public string ClientId { get; set; } = default!;
        
        public string ProtocolType { get; set; } = "oidc";
        
        public List<ClientSecretDto> ClientSecrets { get; set; } = new();
        
        public bool RequireClientSecret { get; set; } = true;
        
        public string ClientName { get; set; } = default!;
        
        public string Description { get; set; } = default!;
        
        public string ClientUri { get; set; } = default!;
        
        public string LogoUri { get; set; } = default!;
        
        public bool RequireConsent { get; set; } = true;
        
        public bool AllowRememberConsent { get; set; } = true;
        
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        
        public List<ClientGrantTypeDto> AllowedGrantTypes { get; set; } = new();

        public bool RequirePkce { get; set; } = true;
        
        public bool AllowPlainTextPkce { get; set; }
        
        public bool RequireRequestObject { get; set; }
        
        public bool AllowAccessTokensViaBrowser { get; set; }
        
        public List<ClientRedirectUriDto> RedirectUris { get; set; } = new();
        
        public List<ClientPostLogoutRedirectUriDto> PostLogoutRedirectUris { get; set; } = new();
        
        public string FrontChannelLogoutUri { get; set; } = default!;

        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        
        public string BackChannelLogoutUri { get; set; } = default!;
        
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        
        public bool AllowOfflineAccess { get; set; }
        
        public List<ClientScopeDto> AllowedScopes { get; set; } = new();
        
        public int IdentityTokenLifetime { get; set; } = 300;
        
        public string AllowedIdentityTokenSigningAlgorithms { get; set; } = default!;
        
        public int AccessTokenLifetime { get; set; } = 3600;
        
        public int AuthorizationCodeLifetime { get; set; } = 300;
        
        public int? ConsentLifetime { get; set; }
        
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        
        public int RefreshTokenUsage { get; set; } =(int)TokenUsage.OneTimeOnly;
        
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        
        public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;
        
        public int AccessTokenType { get; set; } = 0;
        
        public bool EnableLocalLogin { get; set; } = true;

        public List<ClientIdPRestrictionDto> IdentityProviderRestrictions { get; set; } = new();
        
        public bool IncludeJwtId { get; set; }
        
        public List<ClientClaimDto> Claims { get; set; } = new();
        
        public bool AlwaysSendClientClaims { get; set; }
        
        public string ClientClaimsPrefix { get; set; } = "client_";
        
        public string PairWiseSubjectSalt { get; set; } = default!;
        
        public List<ClientCorsOriginDto> AllowedCorsOrigins { get; set; } = new();
        
        public List<ClientPropertyDto> Properties { get; set; } = new();
        
        public int? UserSsoLifetime { get; set; }
        
        public string UserCodeType { get; set; } = default!;
        
        public int DeviceCodeLifetime { get; set; } = 300;
        
        public int? CibaLifetime { get; set; }
        public int? PollingInterval { get; set; }

        public bool? CoordinateLifetimeWithUserSession { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        
        public bool NonEditable { get; set; }
}