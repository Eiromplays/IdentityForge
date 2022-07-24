using Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class AddRoleClaimModels
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public AddRoleClaimRequest AddRoleClaimRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}