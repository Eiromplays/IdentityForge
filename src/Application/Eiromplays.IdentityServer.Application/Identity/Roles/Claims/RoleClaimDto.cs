using Eiromplays.IdentityServer.Application.Identity.Claims;

namespace Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

public class RoleClaimDto
{
    public int Id { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public ClaimDto Claim { get; set; } = default!;
}