using System.ComponentModel.DataAnnotations;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public bool Remember { get; set; }
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
        public string? ValidReturnUrl { get; set; }
    }

    [Route("spa")]
    [AllowAnonymous]
    public class SpaEndpoints : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IServerUrls _serverUrls;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserResolver<ApplicationUser> _userResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SpaEndpoints(IIdentityServerInteractionService interaction, IServerUrls serverUrls,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IUserResolver<ApplicationUser> userResolver, IHttpContextAccessor httpContextAccessor)
        {
            _interaction = interaction;
            _serverUrls = serverUrls;
            _userManager = userManager;
            _signInManager = signInManager;
            _userResolver = userResolver;
            _httpContextAccessor = httpContextAccessor;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var response = new LoginConsentResponse();

            if (ModelState.IsValid)
            {
                var user = await _userResolver.GetUserAsync(model.Login);

                if (user is not null)
                {
                    var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.Remember, lockoutOnFailure: true);

                    if (!loginResult.Succeeded)
                    {
                        response.Error = "Invalid username or password";
                        return new BadRequestObjectResult(response);
                    }

                    var url = model.ReturnUrl != null ? Uri.UnescapeDataString(model.ReturnUrl) : null;

                    var authzContext = await _interaction.GetAuthorizationContextAsync(url);
                    response.ValidReturnUrl = authzContext != null ? url : _serverUrls.BaseUrl;

                    return Ok(response);
                }
            }

            response.Error = "invalid username or password";
            return new BadRequestObjectResult(response);
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logoutInfo = await _interaction.GetLogoutContextAsync(logoutId);

            if (logoutInfo == null)
                return Ok(new
                {
                    prompt = User.Identity?.IsAuthenticated
                });
            
            if (User.Identity != null && logoutInfo.ShowSignoutPrompt && User.Identity.IsAuthenticated)
                return Ok(new
                {
                    prompt = User.Identity.IsAuthenticated
                });
            await HttpContext.SignOutAsync();

            return Ok(new
            {
                iframeUrl = logoutInfo.SignOutIFrameUrl,
                postLogoutRedirectUri = logoutInfo.PostLogoutRedirectUri
            });

        }

        [HttpPost("logout")]
        public async Task<IActionResult> PostLogout(string logoutId)
        {
            var logoutInfo = await _interaction.GetLogoutContextAsync(logoutId);

            await HttpContext.SignOutAsync();

            return Ok(new
            {
                iframeUrl = logoutInfo?.SignOutIFrameUrl,
                postLogoutRedirectUri = logoutInfo?.PostLogoutRedirectUri
            });
        }
    }
}
