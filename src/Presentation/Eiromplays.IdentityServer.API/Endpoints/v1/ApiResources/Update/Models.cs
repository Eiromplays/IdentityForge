using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Update;

public class Models
{
    public class Request
    {
        public int Id { get; set; }
        public UpdateApiResourceRequest Data { get; set; } = default!;
    }
}