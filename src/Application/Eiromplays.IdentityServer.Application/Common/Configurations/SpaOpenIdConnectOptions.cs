using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class SpaOpenIdConnectOptions
{
    public string Authority { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;

    public string ResponseType { get; set; } = OpenIdConnectResponseType.IdToken;

    public string ResponseMode { get; set; } = OpenIdConnectResponseMode.FormPost;

    public bool GetClaimsFromUserInfoEndpoint { get; set; }

    public bool MapInboundClaims { get; set; }

    public bool SaveTokens { get; set; }

    public List<string> Scope { get; set; } = new();

    public List<SpaClaimAction> ClaimActions { get; set; } = new();
}

public class SpaClaimAction
{
    public string ClaimType { get; set; } = default!;

    public string JsonKey { get; set; } = default!;

    public string ValueType { get; set; } = "http://www.w3.org/2001/XMLSchema#string";

    public bool IsUnique { get; set; } = false;

    public ClaimAction MapToClaimAction()
    {
        return IsUnique
            ? new UniqueJsonKeyClaimAction(ClaimType, ValueType, JsonKey)
            : new JsonKeyClaimAction(ClaimType, ValueType, JsonKey);
    }
}