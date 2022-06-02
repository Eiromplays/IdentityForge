using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.UpdateClient;

public class Models
{
    public class Request
    {
        public int Id { get; set; }
        public UpdateClientRequest Data { get; set; } = default!;
    }
}