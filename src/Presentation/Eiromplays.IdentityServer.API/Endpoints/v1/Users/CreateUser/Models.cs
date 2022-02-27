using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints.Validation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Models
{
    public class Request
    {
        public UserDto? UserDto { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserDto!.UserName)
                .NotNull()
                .WithMessage("UserName is required")
                .NotEmpty()
                .WithMessage("UserName cannot be empty");
        }
    }
    
    public class Response
    {
        public UserDto? UserDto { get; set; }
    }
}