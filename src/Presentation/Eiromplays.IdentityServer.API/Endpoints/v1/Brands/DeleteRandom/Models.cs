using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.DeleteRandom;

public class Models
{
    public class Request
    {
        public GenerateRandomBrandRequest GenerateRandomBrandRequest { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }
    }
}