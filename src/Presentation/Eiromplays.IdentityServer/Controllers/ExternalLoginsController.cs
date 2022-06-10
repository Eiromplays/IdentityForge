using Duende.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Controllers;

[SecurityHeaders]
[Microsoft.AspNetCore.Authorization.Authorize]
public class ExternalLoginsController : Controller
{
    /*private readonly IUserService _userService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly SpaConfiguration _spaConfiguration;

    public ExternalLoginsController(IUserService userService, SignInManager<ApplicationUser> signInManager, IOptions<SpaConfiguration> spaConfiguration)
    {
        _userService = userService;
        _signInManager = signInManager;
        _spaConfiguration = spaConfiguration.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.GetSubjectId();
        
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required");

        return Ok(await _userService.GetExternalLoginsAsync(userId));
    }

    [HttpGet("[controller]/add-login-callback")]
    public async Task<IActionResult> AddLoginCallback()
    {
        var userId = User.GetSubjectId();
        
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required");
        
        var info = await _signInManager.GetExternalLoginInfoAsync(userId);

        if (info is null) return Redirect($"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/login");
        
        await _userService.AddLoginAsync(userId, info.Adapt<UserLoginInfoDto>());
        
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return Redirect($"{_spaConfiguration.IdentityServerUiBaseUrl}/app/user-logins");
    }
    
    [HttpGet("[controller]/add-login")]
    public async Task<IActionResult> AddLogin(string provider)
    {
        var userId = User.GetSubjectId();
        
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Action(nameof(AddLoginCallback));
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        
        return Challenge(properties, provider);
    }*/
}