using Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

namespace Eiromplays.IdentityServer.Application.Identity.Roles;

public interface IRoleService : ITransientService
{
    Task<PaginationResponse<RoleDto>> SearchAsync(RoleListFilter filter, CancellationToken cancellationToken = default);
    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<RoleDto> GetByIdAsync(string id);

    Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken = default);

    Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

    Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken = default);

    Task<string> DeleteAsync(string id);

    #region Claims

    Task<PaginationResponse<RoleClaimDto>> SearchClaims(RoleClaimListFilter filter, CancellationToken cancellationToken);

    Task<List<RoleClaimDto>> GetClaimsAsync(string roleId);

    Task<string> AddClaimAsync(string roleId, AddRoleClaimRequest request);

    Task<string> RemoveClaimAsync(string roleId, int claimId);

    Task<string> UpdateClaimAsync(string roleId, int claimId, UpdateRoleClaimRequest request);

    #endregion
}