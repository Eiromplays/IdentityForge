namespace Eiromplays.IdentityServer.Application.Common.Configurations.Identity;

public class Role
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<Claim> Claims { get; set; } = new();
}