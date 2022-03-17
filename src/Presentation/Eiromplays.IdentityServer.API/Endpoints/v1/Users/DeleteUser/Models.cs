using Eiromplays.IdentityServer.Application.Common.Models;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DeleteUser;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
    }

    public class Response
    {
        public Result Result { get; set; }
    }
}