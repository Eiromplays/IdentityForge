using Eiromplays.IdentityServer.Application.Catalog.Brands;
using Eiromplays.IdentityServer.Application.Common.Models;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.Search;

public class Models
{
    public class Request
    {
        public SearchBrandsRequest SearchBrandsRequest { get; set; }
    }

    public class Response
    {
        public PaginationResponse<BrandDto> Brands { get; set; }
    }
}