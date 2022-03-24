using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints;
using FastEndpoints.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
        
        public string? UserName { get; set; }
        
        public string? Email { get; set; }
        
        public string? GravatarEmail { get; set; }

        [BindFrom("revokeUserSessions")] public bool RevokeUserSessions { get; set; } = true;
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .WithMessage("UserName is required")
                .NotEmpty()
                .WithMessage("UserName cannot be empty");
            
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email is not valid")
                .NotNull()
                .WithMessage("Email is required")
                .NotEmpty()
                .WithMessage("Email cannot be empty");

            RuleFor(x => x.GravatarEmail)
                .EmailAddress()
                .WithMessage("Gravatar Email is not valid")
                .When(x => !string.IsNullOrEmpty(x.GravatarEmail));
        }
    }
    
    public class Response
    {
        public UserDto? UserDto { get; set; }
    }
}