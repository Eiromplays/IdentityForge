using Eiromplays.IdentityServer.Application.DTOs.Role;
using FastEndpoints.Validation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.UpdateRole;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
        
        public string? Name { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("UserName is required")
                .NotEmpty()
                .WithMessage("UserName cannot be empty");
        }
    }
    
    public class Response
    {
        public RoleDto? RoleDto { get; set; }
    }
}