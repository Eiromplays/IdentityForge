using System.Security.Claims;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using FastEndpoints;
using LanguageExt.Common;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using LogoutRequest = Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login.LogoutRequest;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ICurrentUser _currentUser;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly IEventService _events;
    private readonly LinkGenerator _linkGenerator;
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly IServerUrls _serverUrls;
    private readonly SpaConfiguration _spaConfiguration;
    private readonly IUserService _userService;

    public AuthService(
        IIdentityServerInteractionService interaction,
        ICurrentUser currentUser,
        SignInManager<ApplicationUser> signInManager,
        IAuthenticationHandlerProvider authenticationHandlerProvider,
        IEventService events,
        LinkGenerator linkGenerator,
        IUserResolver<ApplicationUser> userResolver,
        IServerUrls serverUrls,
        IOptions<SpaConfiguration> spaConfiguration,
        IUserService userService,
        UserManager<ApplicationUser> userManager)
    {
        _interaction = interaction;
        _currentUser = currentUser;
        _signInManager = signInManager;
        _authenticationHandlerProvider = authenticationHandlerProvider;
        _events = events;
        _linkGenerator = linkGenerator;
        _userResolver = userResolver;
        _serverUrls = serverUrls;
        _userService = userService;
        _userManager = userManager;
        _spaConfiguration = spaConfiguration.Value;
    }

    #region Login

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();

        var provider = Enum.Parse<AccountProviders>(request.Provider, true);

        var user = await _userResolver.GetUserAsync(request.Login);
        if (user is null)
        {
            var badRequest = new BadRequestException("Invalid username or password");
            return new Result<LoginResponse>(badRequest);
        }

        var loginResult = provider is AccountProviders.Email ? await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true) : await _userManager.VerifyChangePhoneNumberTokenAsync();
        response.SignInResult = loginResult;

        if (!loginResult.Succeeded)
        {
            if (loginResult.RequiresTwoFactor)
            {
                response.TwoFactorReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/login2fa/{request.RememberMe}/{request.ReturnUrl}";
                return new Result<LoginResponse>(response);
            }

            if (loginResult.IsLockedOut)
            {
                // TODO: Option to send email to user to notify them of their account being locked
                return new Result<LoginResponse>(response);
            }

            response.Error = "Invalid username or password";
            return response;
        }

        var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

        response.ValidReturnUrl = context is not null ? request.ReturnUrl : _serverUrls.BaseUrl;
        return response;
    }

    public async Task<Result<LoginResponse>> Login2FaAsync(Login2FaRequest request)
    {
        var response = new LoginResponse();

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
            throw new InvalidOperationException("Unable to get user");

        string authenticatorCode = request.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result =
            await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, request.RememberMe, request.RememberMachine);

        if (!result.Succeeded) return new Result<LoginResponse>(new BadRequestException("Invalid authentication code"));

        response.SignInResult = result;

        string? url = !string.IsNullOrWhiteSpace(request.ReturnUrl) ? Uri.UnescapeDataString(request.ReturnUrl) : null;

        if (_interaction.IsValidReturnUrl(url))
            response.ValidReturnUrl = url ?? _serverUrls.BaseUrl;

        return new Result<LoginResponse>(response);
    }

    #endregion

    #region Logout

    public async Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId, bool showLogoutPrompt = true)
        {
            var response = new GetLogoutResponse { LogoutId = logoutId, ShowLogoutPrompt = showLogoutPrompt };

            if (!_currentUser.IsAuthenticated())
            {
                // if the user is not authenticated, then just show logged out page
                response.ShowLogoutPrompt = false;
                return response;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            if (context?.ShowSignoutPrompt != false)
                return response;

            // it's safe to automatically sign-out
            response.ShowLogoutPrompt = false;
            return response;
        }

    public async Task<LogoutResponse> LogoutAsync<TEndpoint>(LogoutRequest request, HttpContext httpContext)
        where TEndpoint : IEndpoint
    {
        // build a response so the logged out page knows what to display
        var response = await BuildLoggedOutResponseAsync(request.LogoutId, httpContext);

        if (_currentUser.IsAuthenticated())
        {
            // delete local authentication cookie
            await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(_currentUser.GetSubjectId(), _currentUser.GetDisplayName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (!response.TriggerExternalSignout)
        {
            return response;
        }

        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        string? url = _linkGenerator.GetUriByName(httpContext, typeof(TEndpoint).EndpointName(), new { logoutId = response.LogoutId });

        // this triggers a redirect to the external provider for sign-out
        await httpContext.SignOutAsync(response.ExternalAuthenticationScheme, new AuthenticationProperties { RedirectUri = url });

        await httpContext.Response.CompleteAsync();
        return response;
    }

    private async Task<LogoutResponse> BuildLoggedOutResponseAsync(string logoutId, HttpContext httpContext, bool automaticRedirectAfterSignOut = true)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LogoutResponse
            {
                AutomaticRedirectAfterSignOut = automaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (!_currentUser.IsAuthenticated()) return vm;

            string? idp = _currentUser.GetIdentityProvider();

            if (idp is null or Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider) return vm;
            var authenticationHandler = await _authenticationHandlerProvider.GetHandlerAsync(httpContext, idp);

            bool providerSupportsSignout = authenticationHandler is IAuthenticationSignOutHandler;

            if (!providerSupportsSignout)
                return vm;

            vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();

            vm.ExternalAuthenticationScheme = idp;

            return vm;
        }

        // Helper methods
    protected virtual SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes) =>
            new(authenticationSchemes, properties);

    #endregion

    #region External Login

    public async Task<Result<AuthenticationProperties>> ExternalLoginAsync<TEndpoint>(ExternalLoginRequest request, HttpResponse rsp)
        where TEndpoint : IEndpoint
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            return new Result<AuthenticationProperties>(new BadRequestException("Provider is required"));

        string? redirectUrl = _linkGenerator.GetUriByName(rsp.HttpContext, typeof(TEndpoint).EndpointName(), new { returnUrl = request.ReturnUrl });

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, redirectUrl);

        // Challenge the user with the specified provider
        await rsp.HttpContext.ChallengeAsync(request.Provider, properties);
        await rsp.CompleteAsync();

        return new Result<AuthenticationProperties>(properties);
    }

    public async Task<Result<LoginResponse>> ExternalLoginCallbackAsync(
        ExternalLoginCallbackRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.RemoteError))
            return new Result<LoginResponse>(new BadRequestException("Error from external provider", new List<string> { request.RemoteError }));

        var response = new LoginResponse();

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/login";
            return new Result<LoginResponse>(response);
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);

        if (result.Succeeded)
        {
            string? url = !string.IsNullOrWhiteSpace(request.ReturnUrl) ? Uri.UnescapeDataString(request.ReturnUrl) : null;

            response.ExternalLoginReturnUrl = url ?? _spaConfiguration.IdentityServerUiBaseUrl;
            return new Result<LoginResponse>(response);
        }

        response.SignInResult = result;
        if (result.RequiresTwoFactor)
        {
            return await Login2FaAsync(new Login2FaRequest { ReturnUrl = request.ReturnUrl, RememberMe = true });
        }

        if (result.IsLockedOut)
        {
            // TODO: should probably send an email to the user

            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/lockout";

            return new Result<LoginResponse>(response);
        }

        if (result.IsNotAllowed)
        {
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/not-allowed";

            return new Result<LoginResponse>(response);
        }

        // If the user does not have an account, then ask the user to create an account.
        string? email = info.Principal.FindFirstValue(ClaimTypes.Email);
        string? userName = info.Principal.Identity?.Name;

        response.ExternalLoginReturnUrl =
            $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/external-login-confirmation/{email}/{userName}/{info.LoginProvider}?returnUrl={request.ReturnUrl}";
        return new Result<LoginResponse>(response);
    }

    public async Task LinkExternalLoginAsync<TEndpoint>(
        LinkExternalLoginRequest request, string userId, HttpResponse rsp)
        where TEndpoint : IEndpoint
    {
        // Clear the existing external cookie to ensure a clean login process
        await rsp.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        string? redirectUrl = _linkGenerator.GetUriByName(rsp.HttpContext, typeof(TEndpoint).EndpointName(), new object());

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, redirectUrl, userId);

        // Challenge the user with the specified provider
        await rsp.HttpContext.ChallengeAsync(request.Provider, properties);
        await rsp.CompleteAsync();
    }

    public async Task<Result<LoginResponse>> LinkExternalLoginCallbackAsync(string userId, HttpContext httpContext)
    {
        var response = new LoginResponse();
        var info = await _signInManager.GetExternalLoginInfoAsync(userId);

        if (info is null)
        {
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/login";
            return new Result<LoginResponse>(response);
        }

        string responseMessage = await _userService.AddLoginAsync(userId, info.Adapt<UserLoginInfoDto>());

        // Clear the existing external cookie to ensure a clean login process
        await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/app/user-logins";
        response.Message = responseMessage;

        return new Result<LoginResponse>(response);
    }

    #endregion
}