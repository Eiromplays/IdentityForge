namespace Eiromplays.IdentityServer.Application.Identity.DTOs.Role;

public class RoleDto<TKey>
{
    public TKey? Id { get; set; }

    public string? Name { get; set; }
}