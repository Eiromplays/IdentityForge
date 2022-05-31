using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Users;
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
        
        Console.WriteLine($"login: {login.LoginProvider} {login.ProviderKey} {login.ProviderDisplayName}");
        
        var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName));
        
        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(",", result.Errors.Select(e => e.Description)));
        }

        return string.Format(_t["Login {0} added."], login.ProviderDisplayName);
    }
}