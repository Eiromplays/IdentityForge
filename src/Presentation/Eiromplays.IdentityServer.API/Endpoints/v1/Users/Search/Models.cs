using Eiromplays.IdentityServer.Application.Catalog.Products;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Search;

public class Models
{
    public class Request
    {
        public UserListFilter UserListFilter { get; set; }
    }

    public class Response
    {
        public PaginationResponse<UserDetailsDto> Users { get; set; }
    }
}