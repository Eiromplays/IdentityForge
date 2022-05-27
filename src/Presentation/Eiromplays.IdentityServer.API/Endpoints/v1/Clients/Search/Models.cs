using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.Search;

public class Models
{
    public class Request
    {
        public ClientListFilter Data { get; set; } = default!;
    }
}