namespace Eiromplays.IdentityServer.Application.Identity.Users.Logins;

public class ExternalLoginsResponse
{
    public List<UserLoginInfoDto> CurrentLogins { get; set; } = new();

    public List<AuthenticationSchemeDto> OtherLogins { get; set; } = new();

    public bool ShowRemoveButton { get; set; }
}