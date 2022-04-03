using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<Dictionary<string, string>> GetPersonalDataAsync(string userId, bool includeLogins = true)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        
        var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

        var personalData = personalDataProps.ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");
        
        var logins = await GetLoginsAsync(user.Id);
        
        foreach (var l in logins.Where(l => !string.IsNullOrWhiteSpace(l.ProviderKey)))
            personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey!);
        
        return personalData;
    }
}