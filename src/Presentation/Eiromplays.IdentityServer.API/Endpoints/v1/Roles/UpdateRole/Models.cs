using Eiromplays.IdentityServer.Application.Identity.Roles;
using FastEndpoints;
using FastEndpoints.Validation;
using FluentValidation;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.UpdateRole;

public class Models
{
    public class Request
    {
        public CreateOrUpdateRoleRequest? RoleDto { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.RoleDto!.Name)
                .NotNull()
                .WithMessage("UserName is required")
                .NotEmpty()
                .WithMessage("UserName cannot be empty");
        }
    }
    
    public class Response
    {
        public string? RoleId { get; set; }
    }
}