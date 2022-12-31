using System.Security.Claims;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Common.Mailing;
using Eiromplays.IdentityServer.Application.Common.Sms;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using FastEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using LogoutRequest = Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login.LogoutRequest;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ICurrentUser _currentUser;
    private readonly ApplicationSignInManager _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly IEventService _events;
    private readonly LinkGenerator _linkGenerator;
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly IServerUrls _serverUrls;
    private readonly SpaConfiguration _spaConfiguration;
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly ISmsService _smsService;
    private readonly IMailService _mailService;
    private readonly IStringLocalizer _t;

    public AuthService(
        IIdentityServerInteractionService interaction,
        ICurrentUser currentUser,
        ApplicationSignInManager signInManager,
        IAuthenticationHandlerProvider authenticationHandlerProvider,
        IEventService events,
        LinkGenerator linkGenerator,
        IUserResolver<ApplicationUser> userResolver,
        IServerUrls serverUrls,
        IOptions<SpaConfiguration> spaConfiguration,
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        IJobService jobService,
        ISmsService smsService,
        IMailService mailService,
        IStringLocalizer<AuthService> t)
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
        _jobService = jobService;
        _smsService = smsService;
        _mailService = mailService;
        _t = t;
        _spaConfiguration = spaConfiguration.Value;
    }

    #region Login

    public async Task<SendSmsLoginCodeResponse> SendLoginVerificationCodeAsync(SendSmsLoginCodeRequest request)
    {
        var user = await _userResolver.GetUserAsync(request.PhoneNumber);

        if (user is null)
        {
            throw new BadRequestException("Invalid phone number");
        }

        if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
        {
            throw new BadRequestException("Phone number is not confirmed");
        }

        string? code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

        var smsRequest = new SmsRequest(
            new List<string> { user.PhoneNumber },
            _t[$"Your verification code is {code}"]);

        _jobService.Enqueue(() => _smsService.SendAsync(smsRequest, CancellationToken.None));

        return new SendSmsLoginCodeResponse
        {
            Message = _t["Verification code has been sent to your phone!"]
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string origin)
    {
        var response = new LoginResponse();

        var provider = Enum.Parse<AccountProviders>(request.Provider, true);

        var user = await _userResolver.GetUserAsync(request.Login);
        if (user is null)
        {
            throw new BadRequestException(provider is AccountProviders.Phone ? "Invalid phone number or code" : "Invalid username or password");
        }

        var loginResult = provider is AccountProviders.Email
            ? await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true)
            : await _signInManager.PhoneNumberSignInAsync(user, request.Code, request.RememberMe, lockoutOnFailure: true);

        response.SignInResult = loginResult;
        var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

        string? returnUrl = context is not null ? request.ReturnUrl : _serverUrls.BaseUrl;

        if (!loginResult.Succeeded)
        {
            if (loginResult.RequiresTwoFactor)
            {
                response.TwoFactorReturnUrl =
                    $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/login2fa?rememberMe={request.RememberMe}&returnUrl={returnUrl}";
                return response;
            }

            if (loginResult.IsLockedOut)
            {
                // TODO: Option to send email to user to notify them of their account being locked
                return response;
            }

            if (loginResult.IsNotAllowed)
            {
                response.ValidReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/not-allowed";
                response = await CheckUserVerifiedAsync(user, response, request.ReturnUrl, origin);

                return response;
            }

            response.Error = provider is AccountProviders.Phone ? "Invalid phone number or code" : "Invalid username or password";
            return response;
        }

        response.ValidReturnUrl = returnUrl;
        return response;
    }

    public async Task<LoginResponse> Login2FaAsync(Login2FaRequest request)
    {
        var response = new LoginResponse();

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
            throw new InvalidOperationException("Unable to get user");

        string authenticatorCode = request.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result =
            await _signInManager.TwoFactorSignInAsync(request.Provider, authenticatorCode, request.RememberMe, request.RememberMachine);

        if (!result.Succeeded) throw new BadRequestException("Invalid authentication code");

        response.SignInResult = result;

        string? url = !string.IsNullOrWhiteSpace(request.ReturnUrl) ? Uri.UnescapeDataString(request.ReturnUrl) : null;

        if (_interaction.IsValidReturnUrl(url))
            response.ValidReturnUrl = url ?? _serverUrls.BaseUrl;

        return response;
    }

    public async Task<Send2FaVerificationCodeResponse> Send2FaVerificationCodeAsync(
        Send2FaVerificationCodeRequest request)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
            throw new BadRequestException("Unable to get user");

        string? code = await _userManager.GenerateTwoFactorTokenAsync(user, request.Provider);
        if (string.IsNullOrWhiteSpace(code))
            throw new InternalServerException("Unable to generate two factor code");

        var message = _t[$"Your verification code is {code}"];

        var messages = new List<string>();

        switch (request.Provider)
        {
            case nameof(TwoFactorAuthenticationProviders.Phone):
                var smsRequest = new SmsRequest(
                        new List<string> { user.PhoneNumber },
                        message);

                _jobService.Enqueue(() => _smsService.SendAsync(smsRequest, CancellationToken.None));

                messages.Add(_t["Verification code has been sent to your phone!"]);
                break;

            case nameof(TwoFactorAuthenticationProviders.Email):
                var emailRequest = new MailRequest(
                        new List<string> { user.Email },
                        _t["Verification code"],
                        message);

                _jobService.Enqueue(() => _mailService.SendAsync(emailRequest, CancellationToken.None));

                messages.Add(_t["Verification code has been sent to your email!"]);
                break;
        }

        return new Send2FaVerificationCodeResponse
        {
            Message = string.Join(Environment.NewLine, messages)
        };
    }

    public async Task<IList<string>> GetValidTwoFactorProvidersAsync()
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
            throw new BadRequestException("Unable to get user");

        var validProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);

        return new List<string>(validProviders);
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

    #endregion

    #region External Login

    public async Task<AuthenticationProperties> ExternalLoginAsync<TEndpoint>(ExternalLoginRequest request, HttpResponse rsp)
        where TEndpoint : IEndpoint
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            throw new BadRequestException("Provider is required");

        var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
        string? redirectUrl = _linkGenerator.GetUriByName(
            rsp.HttpContext,
            typeof(TEndpoint).EndpointName(),
            new { returnUrl = context is not null ? request.ReturnUrl : _serverUrls.BaseUrl });

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, redirectUrl);

        // Challenge the user with the specified provider
        await rsp.HttpContext.ChallengeAsync(request.Provider, properties);
        await rsp.CompleteAsync();

        return properties;
    }

    public async Task<LoginResponse> ExternalLoginCallbackAsync(
        ExternalLoginCallbackRequest request, string origin)
    {
        if (!string.IsNullOrWhiteSpace(request.RemoteError))
            throw new BadRequestException("Error from external provider", new List<string> { request.RemoteError });

        var response = new LoginResponse();

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/login";
            return response;
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);

        var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

        string returnUrl = context is not null ? request.ReturnUrl : _spaConfiguration.IdentityServerUiBaseUrl;

        if (result.Succeeded)
        {
            response.ExternalLoginReturnUrl = returnUrl;
            return response;
        }

        response.SignInResult = result;
        if (result.RequiresTwoFactor)
        {
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/login2fa?rememberMe=true&returnUrl={request.ReturnUrl}";
            return response;
        }

        if (result.IsLockedOut)
        {
            // TODO: We should probably send an email to the user to notify them that their account is locked out and how to unlock it.
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/lockout";

            return response;
        }

        if (result.IsNotAllowed)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/not-allowed";

            if (user is not null)
            {
                response = await CheckUserVerifiedAsync(user, response, request.ReturnUrl, origin);
            }

            return response;
        }

        // If the user does not have an account, then ask the user to create an account.
        string? email = info.Principal.FindFirstValue(ClaimTypes.Email);
        string? userName = info.Principal.Identity?.Name;

        response.ExternalLoginReturnUrl =
            $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/external-login-confirmation?email={email}&username={userName}&loginProvider={info.LoginProvider}&returnUrl={returnUrl}";
        return response;
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

    public async Task<LoginResponse> LinkExternalLoginCallbackAsync(string userId, HttpContext httpContext)
    {
        var response = new LoginResponse();
        var info = await _signInManager.GetExternalLoginInfoAsync(userId);

        if (info is null)
        {
            response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/login";
            return response;
        }

        string responseMessage = await _userService.AddLoginAsync(userId, info.Adapt<UserLoginInfoDto>());

        // Clear the existing external cookie to ensure a clean login process
        await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        response.ExternalLoginReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/app/user-logins";
        response.Message = responseMessage;

        return response;
    }

    #endregion

    #region Login Helper Methods

    private async Task<LoginResponse> CheckUserVerifiedAsync(ApplicationUser user, LoginResponse response, string? returnUrl, string origin)
    {
        if (_signInManager.Options.SignIn.RequireConfirmedEmail &&
            !string.IsNullOrWhiteSpace(user.Email) && !user.EmailConfirmed)
        {
            await _userService.ResendEmailVerificationAsync(
                new ResendEmailVerificationRequest
            {
                Email = user.Email
            }, origin);

            response.ValidReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/confirm-email";
            response.ExternalLoginReturnUrl = response.ValidReturnUrl;

            return response;
        }

        if (!_signInManager.Options.SignIn.RequireConfirmedPhoneNumber &&
            (string.IsNullOrWhiteSpace(user.PhoneNumber) || user.PhoneNumberConfirmed))
        {
            return response;
        }

        await _userService.ResendPhoneNumberVerificationAsync(
            new ResendPhoneNumberVerificationRequest
            {
                PhoneNumber = user.PhoneNumber
            });

        response.ValidReturnUrl =
            $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/verify-phone-number?userId={user.Id}&returnUrl={returnUrl}";
        response.ExternalLoginReturnUrl = response.ValidReturnUrl;

        return response;
    }

    #endregion
}