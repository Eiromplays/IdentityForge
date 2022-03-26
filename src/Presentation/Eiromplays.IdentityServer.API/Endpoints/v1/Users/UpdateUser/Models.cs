using Eiromplays.IdentityServer.Application.Identity.Users;
using FastEndpoints;
using FastEndpoints.Validation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
        
        public UpdateUserRequest UpdateUserRequest { get; set; } = null!;

        [BindFrom("revokeUserSessions")] public bool RevokeUserSessions { get; set; } = true;
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }
    
    public class Response
    {
        
    }
}