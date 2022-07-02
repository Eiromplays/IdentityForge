using Eiromplays.IdentityServer.Application.Common.Exceptions;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<string> DisableTwoFactorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var result = await _userManager.SetTwoFactorEnabledAsync(user, false);

        return result.Succeeded
            ? _t["Two-factor Disabled Successfully!"]
            : throw new InternalServerException(_t["An Error has occurred!", result.GetErrors(_t)]);
    }
}