namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations.Identity;

public class IdentityData
{
    public List<Role> Roles { get; set; } = new();

    public List<User> Users { get; set; } = new();
}