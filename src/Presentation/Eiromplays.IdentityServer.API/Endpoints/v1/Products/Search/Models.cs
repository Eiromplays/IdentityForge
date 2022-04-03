using Eiromplays.IdentityServer.Application.Catalog.Brands;
using Eiromplays.IdentityServer.Application.Catalog.Products;
using Eiromplays.IdentityServer.Application.Common.Models;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Search;

public class Models
{
    public class Request
    {
        public SearchProductsRequest SearchProductsRequest { get; set; }
    }

    public class Response
    {
        public PaginationResponse<ProductDto> Products { get; set; }
    }
}