using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Domain.Constants;
using Microsoft.AspNetCore.Authentication;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetLoginEndpoint : Endpoint<GetLoginRequest, GetLoginResponse>
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IClientStore _clientStore;
    private readonly IIdentityProviderStore _identityProvider;

    public GetLoginEndpoint(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IClientStore clientStore,
        IIdentityProviderStore identityProvider)
    {
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _clientStore = clientStore;
        _identityProvider = identityProvider;
    }

    public override void Configure()
    {
        Get("/account/login");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get login information";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLoginRequest req, CancellationToken ct)
    {
        Response = await BuildLoginResponseAsync(req.ReturnUrl);

        await SendOkAsync(Response, cancellation: ct);
    }

    private async Task<GetLoginResponse> BuildLoginResponseAsync(string returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context?.IdP is not null && await _schemeProvider.GetSchemeAsync(context.IdP) is not null)
        {
            bool local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var response = new GetLoginResponse
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Login = context.LoginHint,
            };

            if (!local)
            {
                response.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
            }

            return response;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName is not null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var dynamicSchemes = (await _identityProvider.GetAllSchemeNamesAsync())
            .Where(x => x.Enabled)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName,
                AuthenticationScheme = x.Scheme
            });

        providers.AddRange(dynamicSchemes);

        bool allowLocal = true;
        if (context?.Client.ClientId is null)
        {
            return new GetLoginResponse
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Login = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

        if (client is not null)
        {
            allowLocal = client.EnableLocalLogin;

            if (client.IdentityProviderRestrictions?.Any() == true)
            {
                providers = providers.Where(provider =>
                    client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        return new GetLoginResponse
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Login = context.LoginHint,
            ExternalProviders = providers.ToArray()
        };
    }
}