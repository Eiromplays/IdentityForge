using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Validators.Account;
using Eiromplays.IdentityServer.ViewModels.Account;
using FluentEmail.Core;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eiromplays.IdentityServer.Controllers
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Login { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
        [MaxLength(2000)]
        public string? ReturnUrl { get; set; }
    }
    
    public class ConsentRequest
    {
        public bool Deny { get; set; }
        public bool Remember { get; set; }
        [MaxLength(2000)]
        public string? ReturnUrl { get; set; }
    }

    public class LoginConsentResponse
    {
        public string? Error { get; set; }
        
        public SignInResult? SignInResult { get; set; }
        
        public string? ValidReturnUrl { get; set; }
    }

    [Route("spa")]
    [AllowAnonymous]
    public class SpaEndpoints : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IServerUrls _serverUrls;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserResolver<ApplicationUser> _userResolver;
        private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
        private readonly IEventService _events;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountConfiguration _accountConfiguration;
        private readonly IFluentEmail _fluentEmail;

        public SpaEndpoints(IIdentityServerInteractionService interaction, IServerUrls serverUrls,
            SignInManager<ApplicationUser> signInManager, IUserResolver<ApplicationUser> userResolver,
            IAuthenticationHandlerProvider authenticationHandlerProvider, IEventService eventService,
            IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider,
            UserManager<ApplicationUser> userManager, IOptions<AccountConfiguration> accountConfiguration,
            IFluentEmail fluentEmail)
        {
            _interaction = interaction;
            _serverUrls = serverUrls;
            _signInManager = signInManager;
            _userResolver = userResolver;
            _authenticationHandlerProvider = authenticationHandlerProvider;
            _events = eventService;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _userManager = userManager;
            _fluentEmail = fluentEmail;
            _accountConfiguration = accountConfiguration.Value;
        }

        [HttpGet("context")]
        public async Task<IActionResult> Context(string returnUrl)
        {
            var authzContext = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (authzContext != null)
            {
                return Ok(new 
                {
                    loginHint = authzContext.LoginHint,
                    idp = authzContext.IdP,
                    tenant = authzContext.Tenant,
                    scopes = authzContext.ValidatedResources.RawScopeValues,
                    client = authzContext.Client.ClientName ?? authzContext.Client.ClientId
                });
            }

            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model, string? returnUrl = null)
        {
            if (_accountConfiguration.RegisterConfiguration is { Enabled: false }) return BadRequest("RegisterFailure");

            returnUrl ??= Url.Content("~/");

            model.DisplayName ??= model.UserName;
            
            var validator = new RegisterValidator(_accountConfiguration);

            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid) return BadRequest(model);

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName ?? model.Email,
                DisplayName = model.DisplayName ?? model.UserName ?? model.Email,
                Email = model.Email
            };

            if (_accountConfiguration.ProfilePictureConfiguration is { Enabled: true, AutoGenerate: true })
                user.ProfilePicture = $"{_accountConfiguration.ProfilePictureConfiguration.DefaultUrl}{user.UserName}.svg";

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded) throw new BadRequestException("", result.Errors.Select(e => e.Description).ToList());
            
            if (await _userManager.IsEmailConfirmedAsync(user))
                return RedirectToAction("RegisterConfirmation", "Account");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var verificationUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code },
                HttpContext.Request.Scheme);

            var confirmEmailViewModel = new ConfirmEmailViewModel
                { Username = user.UserName, VerificationUrl = verificationUrl };

            var sendEmailResponse = await _fluentEmail
                .To(user.Email)
                .Subject("Email Confirmation")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Views/Shared/Templates/Email/ConfirmEmail.cshtml", confirmEmailViewModel)
                .SendAsync();

            if (_signInManager.Options.SignIn.RequireConfirmedAccount && sendEmailResponse.Successful)
            {
                return NoContent();
            }

            if (!sendEmailResponse.Successful)
            {
                throw new InternalServerException("", sendEmailResponse.ErrorMessages.ToList());
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }
        
        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);
            
            if (vm.EnableLocalLogin == false && vm.ExternalProviders.Count() == 1)
            {
                // only one option for logging in
                return ExternalLogin(vm.ExternalProviders.First().AuthenticationScheme, returnUrl);
            }

            return Ok(vm);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var response = new LoginConsentResponse();

            if (ModelState.IsValid)
            {
                var user = await _userResolver.GetUserAsync(model.Login);

                if (user is not null)
                {
                    var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
                    
                    if (!loginResult.Succeeded)
                    {
                        response.SignInResult = loginResult;
                        if (loginResult.RequiresTwoFactor)
                        {
                            return RedirectToAction(nameof(LoginWith2Fa), new { model.ReturnUrl, RememberMe = model.RememberMe });
                        }

                        if (loginResult.IsLockedOut)
                        {
                            // TODO: should probably send an email to the user

                            return BadRequest(response);
                        }
                        
                        response.Error = "Invalid username or password";
                        return new BadRequestObjectResult(response);
                    }
                    
                    var url = model.ReturnUrl != null ? Uri.UnescapeDataString(model.ReturnUrl) : null;
                    var context = await _interaction.GetAuthorizationContextAsync(url);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        response.ValidReturnUrl = url;
                        
                        return Ok(response);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        response.ValidReturnUrl = _serverUrls.BaseUrl;
                    }

                    return Ok(response);
                }
            }

            response.Error = "invalid username or password";
            return new BadRequestObjectResult(response);
        }
        
        [HttpGet("loginWith2fa")]
        public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string? returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user is null)
            {
                throw new InvalidOperationException("Unable to get user");
            }

            var model = new LoginWith2FaViewModel
            {
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };

            return Ok(model);
        }

        [HttpPost("loginWith2fa")]
        public async Task<IActionResult> LoginWith2Fa([FromBody] LoginWith2FaViewModel model)
        {
            var response = new LoginConsentResponse();
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                throw new BadRequestException("", errors);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user is null)
                throw new InvalidOperationException("Unable to get user");
            
            var authenticatorCode = model.TwoFactorCode?.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result =
                await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                var url = model.ReturnUrl != null ? Uri.UnescapeDataString(model.ReturnUrl) : null;

                // TODO: See if I can improve this
                if (_interaction.IsValidReturnUrl(url))
                    response.ValidReturnUrl = url ?? _serverUrls.BaseUrl;

                return Ok(response);
            }

            response.Error = "Invalid authentication code";

            return BadRequest(response);
        }
        
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
                throw new BadRequestException("Error from external provider", new List<string> { remoteError });
            
            var response = new LoginConsentResponse();
            
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return Redirect("https://localhost:3000/auth/login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);
            if (result.Succeeded)
            {
                var url = returnUrl != null ? Uri.UnescapeDataString(returnUrl) : null;
                return Redirect(url ?? Request.GetDisplayUrl());
            }
            response.SignInResult = result;
            if (result.RequiresTwoFactor)
            {
                return BadRequest(response);
            }

            if (result.IsLockedOut)
            {
                // TODO: should probably send an email to the user

                return BadRequest(response);
            }

            // If the user does not have an account, then ask the user to create an account.
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = info.Principal.Identity?.Name;
            
            return Redirect(
                $"https://localhost:3000/auth/external-login-confirmation/{email}/{userName}/{info.LoginProvider}?returnUrl={returnUrl}");
        }
        
        [HttpPost("ExternalLogin")]
        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(string? provider, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(provider))
                throw new BadRequestException("Provider is required");
            
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }
        
        [HttpPost("ExternalLoginConfirmation")]
        public async Task<IActionResult> ExternalLoginConfirmation([FromBody]ExternalLoginConfirmationViewModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("ExternalLoginFailure");
            }

            if (!ModelState.IsValid) return BadRequest(model);
            
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                DisplayName = model.DisplayName ?? model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) throw new BadRequestException("Unable to create user", result.Errors.Select(x => x.Description).ToList());
                
            result = await _userManager.AddLoginAsync(user, info);
                
            if (!result.Succeeded) throw new BadRequestException("Unable to add login", result.Errors.Select(x => x.Description).ToList());
                
            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(returnUrl);

        }
        
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);
            
            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }
            
            return Ok(new LogoutResponse(vm));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId!);

            if (User.Identity?.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (!vm.TriggerExternalSignout) return Ok(new LogoutResponse(null, vm));
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.
            var url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            // this triggers a redirect to the external provider for sign-out
            return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme!);
        }

        [HttpPost("consent")]
        public async Task<IActionResult> Consent([FromBody] ConsentRequest model)
        {
            var response = new LoginConsentResponse();

            if (ModelState.IsValid)
            {
                if (model.ReturnUrl != null)
                {
                    var url = Uri.UnescapeDataString(model.ReturnUrl);

                    var authzContext = await _interaction.GetAuthorizationContextAsync(url);
                    if (authzContext != null)
                    {
                        response.ValidReturnUrl = url;

                        if (model.Deny)
                        {
                            await _interaction.DenyAuthorizationAsync(authzContext, AuthorizationError.AccessDenied);
                        }
                        else
                        {
                            await _interaction.GrantConsentAsync(authzContext,
                                new ConsentResponse
                                {
                                    RememberConsent = model.Remember,
                                    ScopesValuesConsented = authzContext.ValidatedResources.RawScopeValues
                                });
                        }
                    
                        return Ok(response);
                    }
                }
            }

            response.Error = "error";
            return new BadRequestObjectResult(response);
        }
        
        [HttpGet("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var errorInfo = await _interaction.GetErrorContextAsync(errorId);
            return Ok(new { 
                errorInfo.Error,
                errorInfo.ErrorDescription
            });
        }

        // HELPER METHODS
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = Uri.UnescapeDataString(returnUrl),
                    Login = context.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
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
                    Login = context?.LoginHint,
                    ExternalProviders = providers.ToArray()
                };
            
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client == null)
                return new LoginViewModel
                {
                    AllowRememberLogin = AccountOptions.AllowRememberLogin,
                    EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                    ReturnUrl = returnUrl,
                    Login = context.LoginHint,
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
                Login = context.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }
        
        
        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };
            
            if (User.Identity?.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            if (context?.ShowSignoutPrompt != false)
                return vm;

            // it's safe to automatically sign-out
            vm.ShowLogoutPrompt = false;
            return vm;
        }
        
        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User.Identity?.IsAuthenticated != true) return vm;

            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp is null or Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider) return vm;

            var authenticationHandler = await _authenticationHandlerProvider.GetHandlerAsync(HttpContext, idp);

            var providerSupportsSignout = authenticationHandler is IAuthenticationSignOutHandler;

            if (!providerSupportsSignout)
                return vm;

            vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();

            vm.ExternalAuthenticationScheme = idp;

            return vm;
        }
    }
}
