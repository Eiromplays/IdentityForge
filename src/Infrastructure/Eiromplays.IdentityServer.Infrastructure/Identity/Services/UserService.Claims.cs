using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<List<UserClaimDto>> GetClaimsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        var claims = await _userManager.GetClaimsAsync(user);

        return claims.Select(claim => new UserClaimDto
            {
                Type = claim.Type,
                Value = claim.Value,
                ValueType = claim.ValueType,
                Issuer = claim.Issuer,
            })
            .ToList();
    }

    // TODO: Add event for claim added
    public async Task<string> AddClaimAsync(string userId, AddUserClaimRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        var result = await _userManager.AddClaimAsync(user, new Claim(request.Type, request.Value, request.ValueType, request.Issuer));

        if (!result.Succeeded)
            throw new InternalServerException(_t["Adding user claim failed"], result.GetErrors(_t));

        return _t["Claim Added Successfully."];
    }

    // TODO: Add event for claim removed
    public async Task<string> RemoveClaimAsync(string userId, RemoveUserClaimRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        var result = await _userManager.RemoveClaimAsync(user, new Claim(request.Type, request.Value));

        if (!result.Succeeded)
            throw new InternalServerException(_t["Removing user claim failed"], result.GetErrors(_t));

        return _t["Claim Removed Successfully."];
    }

    // TODO: Add event for claim updated
    public async Task<string> UpdateClaimAsync(string userId, UpdateUserClaimRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        var result = await _userManager.ReplaceClaimAsync(user, new Claim(request.OldType, request.OldValue), new Claim(request.NewType, request.NewValue));

        if (!result.Succeeded)
            throw new InternalServerException(_t["Updating user claim failed"], result.GetErrors(_t));

        return _t["Claim Updated Successfully."];
    }
}