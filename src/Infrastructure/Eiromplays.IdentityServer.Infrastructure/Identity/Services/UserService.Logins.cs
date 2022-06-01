using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<List<UserLoginInfoDto>> GetLoginsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        return (await _userManager.GetLoginsAsync(user)).Adapt<List<UserLoginInfoDto>>();
    }
    
    public async Task<string> AddLoginAsync(string userId, UserLoginInfoDto login)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName));
        
        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(",", result.Errors.Select(e => e.Description)));
        }

        return string.Format(_t["Login {0} added."], login.ProviderDisplayName);
    }

    public async Task<ExternalLoginsResponse> GetExternalLoginsAsync(string userId)
    {
        var response = new ExternalLoginsResponse
        {
            CurrentLogins = await GetLoginsAsync(userId),
        };
        
        response.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => response.CurrentLogins.All(ul => auth.Name != ul.LoginProvider)).ToList().Adapt<List<AuthenticationSchemeDto>>();
        response.ShowRemoveButton = response.CurrentLogins.Count > 1 || await HasPasswordAsync(userId);
        
        return response;
    }

    public async Task<string> RemoveLoginAsync(RemoveLoginRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        
        var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to remove login.");
        }
        
        await _signInManager.RefreshSignInAsync(user);
        
        return string.Format(_t["Login provider {0} removed."], model.LoginProvider);
    }
}