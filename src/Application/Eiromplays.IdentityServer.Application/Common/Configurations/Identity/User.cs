namespace Eiromplays.IdentityServer.Application.Common.Configurations.Identity;

public class User
{
    public string UserName { get; set; }  = "";

    public string DisplayName { get; set; }  = "";

    public string Email { get; set; }  = "";

    public string Password {  get; set; }  = "";

    public List<Claim> Claims { get; set; } = new();

    public List<string> Roles { get; set; } = new();
}