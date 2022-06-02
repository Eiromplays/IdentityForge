using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.GenerateRandom;

public class Models
{
    public class Request
    {
        public GenerateRandomBrandRequest Data { get; set; } = default!;
    }

    public class Response
    {
        public string Message { get; set; } = default!;
    }
}