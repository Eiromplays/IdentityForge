using FluentValidation;

namespace Eiromplays.IdentityServer.Application.Permissions.Commands.CreatePermission;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200)
            .NotEmpty()
            .NotNull();
    }
}