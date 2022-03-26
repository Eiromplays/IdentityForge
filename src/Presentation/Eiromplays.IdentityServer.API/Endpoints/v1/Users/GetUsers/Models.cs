using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

public class Models
{
    public class Request : UserListFilter
    {
        
    }

    public class Response
    {
        public PaginationResponse<UserDetailsDto>? Users { get; set; }
    }
}