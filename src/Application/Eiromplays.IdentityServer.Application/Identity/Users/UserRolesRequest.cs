namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UserRolesRequest
{
    public List<UserRoleDto> UserRoles { get; set; } = new();

    public bool RevokeUserSessions { get; set; } = true;
}