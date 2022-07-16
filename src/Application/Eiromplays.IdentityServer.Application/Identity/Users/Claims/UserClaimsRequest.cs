namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class UserClaimsRequest
{
    public List<UserClaimDto> UserClaims { get; set; } = new();

    public bool RevokeUserSessions { get; set; } = true;
}