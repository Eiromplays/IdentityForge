namespace Eiromplays.IdentityServer.Application.Identity.DTOs.User;

public class UserClaimDto
{
    public int ClaimId { get; set; }

    public string? UserId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}