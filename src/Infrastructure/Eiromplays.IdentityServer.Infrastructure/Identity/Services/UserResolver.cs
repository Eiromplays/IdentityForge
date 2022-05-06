﻿using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class UserResolver : IUserResolver<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly LoginPolicy _loginPolicy;

    public UserResolver(UserManager<ApplicationUser> userManager, IOptions<AccountConfiguration> accountConfigurationOptions)
    {
        _userManager = userManager;
        _loginPolicy = accountConfigurationOptions.Value.LoginConfiguration.LoginPolicy;
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