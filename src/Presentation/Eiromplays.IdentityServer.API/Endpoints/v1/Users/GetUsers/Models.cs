using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.DTOs.User;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

public class Models
{
    public class Request
    {
        public string? Search { get; set; }
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

    public class Response
    {
        public PaginatedList<UserDto> Users { get; set; } = new();
    }
}