namespace Eiromplays.IdentityServer.Application.Common.Configurations.Identity;

public class IdentityData
{
    public List<Role> Roles { get; set; } = new();

    public List<User> Users { get; set; } = new();
}