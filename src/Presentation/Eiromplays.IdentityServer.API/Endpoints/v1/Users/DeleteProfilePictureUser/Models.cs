using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints.Validation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DeleteProfilePictureUser;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = "";
    }

    public class Response
    {
        public UserDto? UserDto { get; set; }
    }
}