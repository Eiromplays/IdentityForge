namespace Eiromplays.IdentityServer.Application.Common.Configurations.Identity;

public class User
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public List<Claim> Claims { get; set; } = new();

    public List<string> Roles { get; set; } = new();
}