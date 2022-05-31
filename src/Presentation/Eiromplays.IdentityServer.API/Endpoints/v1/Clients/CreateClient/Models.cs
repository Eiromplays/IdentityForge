using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.CreateClient;

public class Models
{
    public class Request
    {
        public CreateClientRequest Data { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}