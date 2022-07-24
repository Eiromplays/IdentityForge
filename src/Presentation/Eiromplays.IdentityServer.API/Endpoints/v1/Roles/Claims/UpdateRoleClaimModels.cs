using Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class UpdateRoleClaimModels
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public int ClaimId { get; set; }

        public UpdateRoleClaimRequest UpdateRoleClaimRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}