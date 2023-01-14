namespace Eiromplays.IdentityServer.Application.Identity.Roles;

public class CreateOrUpdateRoleRequest
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateOrUpdateRoleRequestValidator : Validator<CreateOrUpdateRoleRequest>
{
    public CreateOrUpdateRoleRequestValidator(IStringLocalizer<CreateOrUpdateRoleRequestValidator> T)
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MustAsync(async (role, name, _) =>
            {
                var roleService = Resolve<IRoleService>();
                return !await roleService.ExistsAsync(name, role.Id);
            })
            .WithMessage(T["Similar Role already exists."]);
    }
}