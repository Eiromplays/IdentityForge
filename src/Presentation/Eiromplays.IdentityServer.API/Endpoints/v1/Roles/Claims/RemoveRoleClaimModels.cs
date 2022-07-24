namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class RemoveRoleClaimModels
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public int ClaimId { get; set; }
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}