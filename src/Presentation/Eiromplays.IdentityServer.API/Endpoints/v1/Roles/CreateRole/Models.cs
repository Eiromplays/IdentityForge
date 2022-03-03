using Eiromplays.IdentityServer.Application.DTOs.Role;
using FastEndpoints.Validation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.CreateRole;

public class Models
{
    public class Request
    {
        public RoleDto? RoleDto { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.RoleDto!.Name)
                .NotNull()
                .WithMessage("Name is required")
                .NotEmpty()
                .WithMessage("Name cannot be empty");
        }
    }
    
    public class Response
    {
        public RoleDto? RoleDto { get; set; }
    }
}