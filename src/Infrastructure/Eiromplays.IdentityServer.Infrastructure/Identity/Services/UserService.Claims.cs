using System.Security.Claims;
using Ardalis.Specification.EntityFrameworkCore;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<PaginationResponse<UserClaimDto>> SearchUserClaimsAsync(UserClaimListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ApplicationUserClaim>(filter);

        var sessions = await _db.UserClaims.WithSpecification(spec).ProjectToType<UserClaimDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.UserClaims
            .CountAsync(cancellationToken);

        return new PaginationResponse<UserClaimDto>(sessions, count, filter.PageNumber, filter.PageSize);
    }

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