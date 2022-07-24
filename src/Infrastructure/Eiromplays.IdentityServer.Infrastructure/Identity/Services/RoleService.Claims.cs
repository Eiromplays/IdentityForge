using Ardalis.Specification.EntityFrameworkCore;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.Roles.Claims;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Claim = System.Security.Claims.Claim;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class RoleService
{
    public async Task<PaginationResponse<RoleClaimDto>> SearchClaims(RoleClaimListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ApplicationRoleClaim>(filter);

        var roleClaims = await _db.RoleClaims
            .Where(uc => uc.RoleId == filter.RoleId || string.IsNullOrWhiteSpace(filter.RoleId))
            .WithSpecification(spec)
            .Select(c => new { c.Id, c.CreatedBy, c.CreatedOn, c.LastModifiedBy, c.LastModifiedOn, Claim = c.ToClaim()})
            .ProjectToType<RoleClaimDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.RoleClaims
            .CountAsync(
                uc => uc.RoleId == filter.RoleId || string.IsNullOrWhiteSpace(filter.RoleId),
                cancellationToken: cancellationToken);

        return new PaginationResponse<RoleClaimDto>(roleClaims, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<List<RoleClaimDto>> GetClaimsAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        _ = role ?? throw new NotFoundException(_t["Role Not Found."]);
        var claims = await _roleManager.GetClaimsAsync(role);

        return claims.Adapt<List<RoleClaimDto>>();
    }

    // TODO: Add event for claim added
    public async Task<string> AddClaimAsync(string roleId, AddRoleClaimRequest request)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        _ = role ?? throw new NotFoundException(_t["Role Not Found."]);
        var result = await _roleManager.AddClaimAsync(role, new Claim(request.Type, request.Value));

        if (!result.Succeeded)
            throw new InternalServerException(_t["Adding user claim failed"], result.GetErrors(_t));

        return _t["Claim Added Successfully."];
    }

    // TODO: Add event for claim removed
    public async Task<string> RemoveClaimAsync(string roleId, int claimId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        _ = role ?? throw new NotFoundException(_t["Role Not Found."]);
        var roleClaim = await _db.RoleClaims.FirstOrDefaultAsync(uc => uc.Id == claimId);
        var result = await _roleManager.RemoveClaimAsync(role, roleClaim?.ToClaim());

        if (!result.Succeeded)
            throw new InternalServerException(_t["Removing role claim failed"], result.GetErrors(_t));

        return _t["Claim Removed Successfully."];
    }

    // TODO: Add event for claim updated
    public async Task<string> UpdateClaimAsync(string roleId, int claimId, UpdateRoleClaimRequest request)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        _ = role ?? throw new NotFoundException(_t["Role Not Found."]);
        var roleClaim = await _db.RoleClaims.FirstOrDefaultAsync(uc => uc.Id == claimId);
        await _roleManager.RemoveClaimAsync(role, roleClaim?.ToClaim());

        var result = await _roleManager.AddClaimAsync(role, new Claim(request.Type, request.Value));

        if (!result.Succeeded)
            throw new InternalServerException(_t["Updating role claim failed"], result.GetErrors(_t));

        return _t["Claim Updated Successfully."];
    }
}