using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Create;

public class Models
{
    public class Request
    {
        public CreateApiResourceRequest Data { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}