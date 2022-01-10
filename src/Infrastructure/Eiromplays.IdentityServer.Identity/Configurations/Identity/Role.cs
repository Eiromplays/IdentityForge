namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations.Identity;

public class Role
{
    public string? Name { get; set; }

    public List<Claim> Claims { get; set; } = new();
}