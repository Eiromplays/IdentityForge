using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Services;

public class UserResolver : IUserResolver<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly LoginPolicy? _loginPolicy;

    public UserResolver(UserManager<ApplicationUser> userManager, AccountConfiguration accountConfiguration)
    {
        _userManager = userManager;
        _loginPolicy = accountConfiguration.LoginConfiguration?.LoginPolicy;
    }

    public async Task<ApplicationUser?> GetUserAsync(string? identifier)
    {
        return _loginPolicy switch
        {
            LoginPolicy.Username => await _userManager.FindByNameAsync(identifier),
            LoginPolicy.Email => await _userManager.FindByEmailAsync(identifier),
            LoginPolicy.Id => await _userManager.FindByIdAsync(identifier),
            LoginPolicy.All => await _userManager.FindByEmailAsync(identifier) ??
                               await _userManager.FindByNameAsync(identifier) ??
                               await _userManager.FindByIdAsync(identifier),
            _ => null
        };
    }
}