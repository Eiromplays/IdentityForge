using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;
using Eiromplays.IdentityServer.Domain.Constants;
using ConsentRequest = Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent.ConsentRequest;
using ConsentResponse = Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent.ConsentResponse;
using IConsentService = Eiromplays.IdentityServer.Application.Identity.Auth.IConsentService;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class ConsentService : IConsentService
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    #region Methods

    public async Task<ConsentResponse> GetConsentAsync(GetConsentRequest request)
    {
        var consentResponse = await BuildResponseAsync(request.ReturnUrl);

        return consentResponse ?? throw new InternalServerException("Failed to build consent view model");
    }

    public async Task<ProcessConsentResponse> ConsentAsync(ConsentRequest request, IPrincipal user)
    {
        var response = await ProcessConsent(request, user);

        if (response.IsRedirect)
        {
            return response;
        }

        if (response.HasValidationError)
            throw new BadRequestException(response.ValidationError);

        return response.ShowResponse
            ? response
            : throw new InternalServerException("Failed to get consent response");
    }

    #endregion

    #region Helper Methods

    public ConsentService(IIdentityServerInteractionService interaction, IEventService events)
    {
        _interaction = interaction;
        _events = events;
    }

    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    private async Task<ProcessConsentResponse> ProcessConsent(ConsentRequest model, IPrincipal user)
    {
        var result = new ProcessConsentResponse();

        // validate return url is still valid
        var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        if (request is null) return result;

        Duende.IdentityServer.Models.ConsentResponse? grantedConsent = null;

        switch (model.Button)
        {
            // user clicked 'no' - send back the standard 'access_denied' response
            case "no":
                grantedConsent = new Duende.IdentityServer.Models.ConsentResponse { Error = AuthorizationError.AccessDenied };

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(user.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
                break;

            // user clicked 'yes' - validate the data
            // if the user consented to some scope, build the response model
            case "yes" when model.ScopesConsented.Any():
                var scopes = model.ScopesConsented;

                if (!ConsentOptions.EnableOfflineAccess)
                #pragma warning disable CS0162
                {
                    scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                }
                #pragma warning restore CS0162

                grantedConsent = new Duende.IdentityServer.Models.ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = model.Description
                };

                // emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(
                    user.GetSubjectId(),
                    request.Client.ClientId,
                    request.ValidatedResources.RawScopeValues,
                    grantedConsent.ScopesValuesConsented,
                    grantedConsent.RememberConsent));
                break;

            case "yes":
                result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                break;
            default:
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
                break;
        }

        if (grantedConsent is not null)
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
            result.Response = await BuildResponseAsync(model.ReturnUrl, model);
        }

        return result;
    }

    private async Task<ConsentResponse?> BuildResponseAsync(string? returnUrl, ConsentRequest? model = null)
    {
        var request = await _interaction.GetAuthorizationContextAsync(returnUrl);

        return request is not null ? CreateConsentResponse(model, returnUrl, request) : null;
    }

    private ConsentResponse CreateConsentResponse(
        ConsentRequest? model,
        string? returnUrl,
        AuthorizationRequest request)
    {
        var response = new ConsentResponse
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

        response.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeResponse(x, response.ScopesConsented.Contains(x.Name) || model is null)).ToArray();

        var apiScopes = (from parsedScope in request.ValidatedResources.ParsedScopes
            let apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName)
            where apiScope is not null
            select CreateScopeResponse(parsedScope, apiScope, response.ScopesConsented.Contains(parsedScope.RawValue) || model is null)).ToList();
        if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(response.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) || model is null));
        }

        response.ApiScopes = apiScopes;

        return response;
    }

    private static ScopeResponse CreateScopeResponse(IdentityResource identity, bool check)
    {
        return new ScopeResponse
        {
            Value = identity.Name,
            DisplayName = identity.DisplayName ?? identity.Name,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    public ScopeResponse CreateScopeResponse(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        string? displayName = apiScope.DisplayName ?? apiScope.Name;

        if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
        {
            displayName += $":{parsedScopeValue.ParsedParameter}";
        }

        return new ScopeResponse
        {
            Value = parsedScopeValue.RawValue,
            DisplayName = displayName,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }

    private static ScopeResponse GetOfflineAccessScope(bool check)
    {
        return new ScopeResponse
        {
            Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }

    #endregion
}