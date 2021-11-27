namespace Eiromplays.IdentityServer.Application.Identity.DTOs.User;

public class UserClaimDto<TKey>
{
    public int ClaimId { get; set; }

    public TKey? UserId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}