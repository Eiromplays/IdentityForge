using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.EndpointDefinitions;

public class AccountEndpointDefinition : IEndpointDefinition
{
    private readonly IIdentityService _identityService;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IEventService _events;

    public AccountEndpointDefinition(IIdentityService identityService, IIdentityServerInteractionService interaction,
        IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider, IEventService events)
    {
        _identityService = identityService;
        _interaction = interaction;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _events = events;
    }

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/account/login/{returnUrl?}",
            async (string? returnUrl) =>
                await LoginAsync(returnUrl));

        app.MapGet("/account/login/{button?}",
            async (HttpContext httpContext, IUrlHelper urlHelper, [FromBody]LoginInputModel model, string? button) =>
                await LoginAsync(httpContext, urlHelper, model, button));
    }

    internal async Task<IResult> LoginAsync(string? returnUrl)
    {
        // Build a model so we know what to show on the login page
        var vm = await BuildLoginViewModelAsync(returnUrl);

        return vm.IsExternalLoginOnly
            ? Results.RedirectToRoute("Challenge", new {scheme = vm.ExternalLoginScheme, returnUrl})
            : Results.Ok(vm);
    }

    internal async Task<IResult> LoginAsync(HttpContext httpContext, IUrlHelper urlHelper, LoginInputModel model, string? button)
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // the user clicked the "cancel" button
        if (button != "login")
        {
            if (context is null) return Results.Redirect("~/");

            // if the user cancels, send a result back into IdentityServer as if they 
            // denied the consent (even if this client does not require consent).
            // this will send back an access denied OIDC error response to the client.
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage("Redirect", model.ReturnUrl);
            }

            return Results.Redirect(model.ReturnUrl!);
        }

        var user = await _identityService.FindUserByUsernameAsync(model.Username);

        if (_users.ValidateCredentials(model.Username, model.Password))
        {
            //await _events.RaiseAsync(new UserLoginSuccessEvent(user?.UserName, user.SubjectId, user.Username, clientId: context?.Client.ClientId));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties? props = null;
            if (AccountOptions.AllowRememberLogin && model.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                };
            };

            // issue authentication cookie with subject ID and username
            var isuser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.UserName
            };

            await httpContext.SignInAsync(isuser, props);

            if (context is not null)
            {
                return context.IsNativeClient() ? httpContext.LoadingPage(model.ReturnUrl!) : Results.Redirect(model.ReturnUrl!);
            }

            // request for a local page
            if (urlHelper.IsLocalUrl(model.ReturnUrl))
            {
                return Results.Redirect(model.ReturnUrl!);
            }

            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Results.Redirect("~/");
            }

            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }

        /*await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Invalid credentials",
            clientId: context?.Client.ClientId));*/

        // something went wrong, show form with error
        var vm = await BuildLoginViewModelAsync(model);
        return Results.Ok(vm);
    }

    /*****************************************/
    /* helper APIs for the AccountEndpointDefinitions */
    /*****************************************/
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string? returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context?.IdP is not null && await _schemeProvider.GetSchemeAsync(context.IdP) is not null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new LoginViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Username = context.LoginHint,
            };

            if (!local)
            {
                vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context?.IdP } };
            }

            return vm;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId == null)
            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };

        var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
        if (client is null)
            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };

        allowLocal = client.EnableLocalLogin;

        if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
        {
            providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
        }

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Username = context?.LoginHint,
            ExternalProviders = providers.ToArray()
        };
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);

        vm.Username = model.Username;
        vm.RememberLogin = model.RememberLogin;
        return vm;
    }
    public void DefineServices(IServiceCollection services)
    {
        
    }
}