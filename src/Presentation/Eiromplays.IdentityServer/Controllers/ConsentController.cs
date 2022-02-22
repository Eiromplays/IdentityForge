// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sj�l�kken

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.ViewModels.Consent;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.Controllers;

/// <summary>
/// This controller processes the consent UI
/// </summary>
[SecurityHeaders]
[Microsoft.AspNetCore.Authorization.Authorize]
[ApiVersion("1.0")]
public class ConsentController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly ILogger<ConsentController> _logger;

    public ConsentController(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<ConsentController> logger)
    {
        _interaction = interaction;
        _events = events;
        _logger = logger;
    }

    /// <summary>
    /// Shows the consent screen
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Index(string returnUrl)
    {
        var vm = await BuildViewModelAsync(returnUrl);
        return vm != null ? View("Index", vm) : View("Error");
    }

    /// <summary>
    /// Handles the consent screen postback
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ConsentInputModel model)
    {
        var result = await ProcessConsent(model);

        if (result.IsRedirect)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            return context?.IsNativeClient() == true ? this.LoadingPage("Redirect", result.RedirectUri!) : Redirect(result.RedirectUri!);
        }

        if (result.HasValidationError)
        {
            ModelState.AddModelError(string.Empty, result.ValidationError!);
        }

        return result.ShowView ? View("Index", result.ViewModel) : View("Error");
    }

    /*****************************************/
    /* helper APIs for the ConsentController */
    /*****************************************/
    private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
    {
        var result = new ProcessConsentResult();

        // validate return url is still valid
        var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        if (request == null) return result;

        ConsentResponse? grantedConsent = null;

        switch (model.Button)
        {
            // user clicked 'no' - send back the standard 'access_denied' response
            case "no":
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
                break;
            // user clicked 'yes' - validate the data
            // if the user consented to some scope, build the response model
            case "yes" when model.ScopesConsented.Any():
            {
                var scopes = model.ScopesConsented;
                if (!ConsentOptions.EnableOfflineAccess)
                {
                    scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = model.Description
                };

                // emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                break;
            }
            case "yes":
                result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                break;
            default:
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
                break;
        }

        if (grantedConsent != null)
        {
            // communicate outcome of consent back to identityserver
            await _interaction.GrantConsentAsync(request, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            result.RedirectUri = model.ReturnUrl;
            result.Client = request.Client;
        }
        else
        {
            // we need to redisplay the consent UI
            result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
        }

        return result;
    }

    private async Task<ConsentViewModel?> BuildViewModelAsync(string? returnUrl, ConsentInputModel? model = null)
    {
        var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (request is not null)
        {
            return CreateConsentViewModel(model, returnUrl, request);
        }

        _logger.LogError("No consent request matching request: {ReturnUrl}", returnUrl);

        return null;
    }

    private ConsentViewModel CreateConsentViewModel(
        ConsentInputModel? model, string? returnUrl,
        AuthorizationRequest request)
    {
        var vm = new ConsentViewModel
        {
            RememberConsent = model?.RememberConsent ?? true,
            ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),
            Description = model?.Description,

            ReturnUrl = returnUrl,

            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            AllowRememberConsent = request.Client.AllowRememberConsent
        };

        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model is null)).ToArray();

        var apiScopes = (from parsedScope in request.ValidatedResources.ParsedScopes
            let apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName)
            where apiScope is not null
            select CreateScopeViewModel(parsedScope, apiScope,
                vm.ScopesConsented.Contains(parsedScope.RawValue) || model is null)).ToList();
        if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(vm.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) || model is null));
        }
        vm.ApiScopes = apiScopes;

        return vm;
    }

    private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
            Value = identity.Name,
            DisplayName = identity.DisplayName ?? identity.Name,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        var displayName = apiScope.DisplayName ?? apiScope.Name;
        if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
        {
            displayName += $":{parsedScopeValue.ParsedParameter}";
        }

        return new ScopeViewModel
        {
            Value = parsedScopeValue.RawValue,
            DisplayName = displayName,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }

    private static ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }
}