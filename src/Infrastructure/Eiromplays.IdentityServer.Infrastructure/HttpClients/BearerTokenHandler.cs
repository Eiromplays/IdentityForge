using System.Net.Http.Headers;
using System.Security.Claims;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Infrastructure.HttpClients;

public class BearerTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IdentityServerTools _identityServerTools;

    public BearerTokenHandler(IHttpContextAccessor httpContextAccessor, IdentityServerTools identityServerTools)
    {
        _httpContextAccessor = httpContextAccessor;
        _identityServerTools = identityServerTools;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null or { User.Identity: null }) return await base.SendAsync(request, cancellationToken);

        var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();

        // The audience claim is required for the token to be valid
        // The scope is just added to the token in case you want to use it in your API :)
        claims.Add(new Claim("aud", "api"));
        claims.Add(new Claim("scope", "api"));

        string issuedJwt =
            await _identityServerTools.IssueJwtAsync(300, claims);

        if (!string.IsNullOrWhiteSpace(issuedJwt))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", issuedJwt);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}