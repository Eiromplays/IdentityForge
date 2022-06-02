namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientRedirectUriDto
{
    public int Id { get; set; }
    public string RedirectUri { get; set; } = default!;
}