using System.Collections;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Identity.Resources;
using ClientClaim = Duende.IdentityServer.EntityFramework.Entities.ClientClaim;
using Secret = Microsoft.AspNetCore.DataProtection.Secret;

namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientDto
{
    private ICollection<string> _allowedGrantTypes = new GrantTypeValidatingHashSet();
    
    public bool Enabled { get; set; } = true;

    public string? ClientId { get; set; }
    
    public string? ProtocolType { get; set; }
    
    public ICollection<SecretDto> ClientSecrets { get; set; } = new HashSet<SecretDto>();
    
    public bool RequireClientSecret { get; set; } = true;
    
    public string? ClientName { get; set; }
    
    public string? Description { get; set; }

    public string? ClientUri { get; set; }


    public string? LogoUri { get; set; }

    public bool RequireConsent { get; set; } = false;


    public bool AllowRememberConsent { get; set; } = true;


    public ICollection<string> AllowedGrantTypes
    {
        get => _allowedGrantTypes;
        set
        {
            ValidateGrantTypes(value);
            _allowedGrantTypes = new GrantTypeValidatingHashSet(value);
        }
    }
    
    public bool RequirePkce { get; set; } = true;
    
    public bool AllowPlainTextPkce { get; set; } = false;
    
    public bool RequireRequestObject { get; set; } = false;

    public bool AllowAccessTokensViaBrowser { get; set; } = false;


    public ICollection<string> RedirectUris { get; set; } = new HashSet<string>();


    public ICollection<string> PostLogoutRedirectUris { get; set; } = new HashSet<string>();


    public string? FrontChannelLogoutUri { get; set; }
    
    public bool FrontChannelLogoutSessionRequired { get; set; } = true;

    public string? BackChannelLogoutUri { get; set; }
    
    public bool BackChannelLogoutSessionRequired { get; set; } = true;
    
    public bool AllowOfflineAccess { get; set; } = false;


    public ICollection<string> AllowedScopes { get; set; } = new HashSet<string>();

    public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

    public int IdentityTokenLifetime { get; set; } = 300;
    
    public ICollection<string> AllowedIdentityTokenSigningAlgorithms { get; set; } = new HashSet<string>();
    
    public int AccessTokenLifetime { get; set; } = 3600;
    
    public int AuthorizationCodeLifetime { get; set; } = 300;
    
    public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
    
    public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
    
    public int? ConsentLifetime { get; set; } = null;
    
    public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;
    
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; } = false;
    
    public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;

    public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;

    public bool EnableLocalLogin { get; set; } = true;

    public ICollection<string> IdentityProviderRestrictions { get; set; } = new HashSet<string>();


    public bool IncludeJwtId { get; set; } = true;

    public ICollection<ClientClaim> Claims { get; set; } = new HashSet<ClientClaim>();

    public bool AlwaysSendClientClaims { get; set; } = false;

    public string ClientClaimsPrefix { get; set; } = "client_";

    public string? PairWiseSubjectSalt { get; set; }

    public int? UserSsoLifetime { get; set; }
    
    public string? UserCodeType { get; set; }

    public int DeviceCodeLifetime { get; set; } = 300;
    
    public int? CibaLifetime { get; set; }

    public int? PollingInterval { get; set; }
    
    public bool? CoordinateLifetimeWithUserSession { get; set; }
    
    public ICollection<string> AllowedCorsOrigins { get; set; } = new HashSet<string>();
    
    public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    
    public static void ValidateGrantTypes(IEnumerable<string> grantTypes)
    {
        if (grantTypes == null)
        {
            throw new ArgumentNullException(nameof(grantTypes));
        }

        // spaces are not allowed in grant types
        if (grantTypes.Any(type => type.Contains(' ')))
        {
            throw new InvalidOperationException("Grant types cannot contain spaces");
        }

        // single grant type, seems to be fine
        if (grantTypes.Count() == 1) return;

        // don't allow duplicate grant types
        if (grantTypes.Count() != grantTypes.Distinct().Count())
        {
            throw new InvalidOperationException("Grant types list contains duplicate values");
        }

        // would allow response_type downgrade attack from code to token
        DisallowGrantTypeCombination(GrantType.Implicit, GrantType.AuthorizationCode, grantTypes);
        DisallowGrantTypeCombination(GrantType.Implicit, GrantType.Hybrid, grantTypes);

        DisallowGrantTypeCombination(GrantType.AuthorizationCode, GrantType.Hybrid, grantTypes);
    }

    private static void DisallowGrantTypeCombination(string value1, string value2, IEnumerable<string> grantTypes)
    {
        if (grantTypes.Contains(value1, StringComparer.Ordinal) &&
            grantTypes.Contains(value2, StringComparer.Ordinal))
        {
            throw new InvalidOperationException($"Grant types list cannot contain both {value1} and {value2}");
        }
    }

    private class GrantTypeValidatingHashSet : ICollection<string>
    {
        private readonly ICollection<string> _inner;

        public GrantTypeValidatingHashSet()
        {
            _inner = new HashSet<string>();
        }

        public GrantTypeValidatingHashSet(IEnumerable<string> values)
        {
            _inner = new HashSet<string>(values);
        }

        private ICollection<string> Clone()
        {
            return new HashSet<string>(this);
        }

        private ICollection<string> CloneWith(params string[] values)
        {
            var clone = Clone();
            foreach (var item in values) clone.Add(item);
            return clone;
        }

        public int Count => _inner.Count;

        public bool IsReadOnly => _inner.IsReadOnly;

        public void Add(string item)
        {
            ValidateGrantTypes(CloneWith(item));
            _inner.Add(item);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(string item)
        {
            return _inner.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public bool Remove(string item)
        {
            return _inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
    }
}