namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientPostLogoutRedirectUriDto
{
    public int Id { get; set; }
    public string PostLogoutRedirectUri { get; set; } = default!;
}