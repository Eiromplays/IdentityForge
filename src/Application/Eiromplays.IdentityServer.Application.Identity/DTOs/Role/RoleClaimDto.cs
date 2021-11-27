using Eiromplays.IdentityServer.Application.Common.Mappings;

namespace Eiromplays.IdentityServer.Application.Identity.DTOs.Role;

public class RoleClaimDto
{
    public int Id { get; set; }

    public string? RoleId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}