using Eiromplays.IdentityServer.Application.Identity.Users;
using FastEndpoints;
using FastEndpoints.Validation;
using FluentValidation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Models
{
    public class Request
    {
        public CreateUserRequest? UserDto { get; set; }
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
        public string? UserId { get; set; }
    }
}