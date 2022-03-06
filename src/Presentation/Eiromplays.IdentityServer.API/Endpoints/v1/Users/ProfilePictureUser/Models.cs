using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints.Validation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ProfilePictureUser;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = "";

        public IFormFile ProfilePicture { get; set; } = null!;
    }
    
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ProfilePicture)
                .NotNull()
                .WithMessage("Profile picture is required");

            RuleFor(x => x.ProfilePicture.Length)
                .GreaterThan(0)
                .WithMessage("Profile picture is required");
        }
    }

    public class Response
    {
        public UserDto? UserDto { get; set; }
    }
}