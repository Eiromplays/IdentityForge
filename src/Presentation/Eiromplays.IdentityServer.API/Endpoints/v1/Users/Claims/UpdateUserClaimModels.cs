using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class UpdateUserClaimModels
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public int ClaimId { get; set; }

        public UpdateUserClaimRequest UpdateUserClaimRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}