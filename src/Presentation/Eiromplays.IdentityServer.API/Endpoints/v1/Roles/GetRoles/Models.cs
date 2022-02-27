using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.DTOs.Role;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRoles;

public class Models
{
    public class Request
    {
        public string? Search { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }

    public class Response
    {
        public PaginatedList<RoleDto> Roles { get; set; } = new();
    }
}