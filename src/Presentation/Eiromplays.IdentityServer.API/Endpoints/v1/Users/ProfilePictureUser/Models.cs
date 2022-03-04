using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ProfilePictureUser;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
        
        public IFormFile? ProfilePicture { get; set; }
    }

    public class Response
    {
        public UserDto? UserDto { get; set; }
    }
}