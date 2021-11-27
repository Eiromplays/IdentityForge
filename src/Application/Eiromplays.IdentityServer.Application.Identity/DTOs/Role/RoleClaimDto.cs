namespace Eiromplays.IdentityServer.Application.Identity.DTOs.Role;

public class RoleClaimDto<TKey>
{
    public int ClaimId { get; set; }

    public TKey? RoleId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}