using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class AddUserClaimModels
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public AddUserClaimRequest AddUserClaimRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}