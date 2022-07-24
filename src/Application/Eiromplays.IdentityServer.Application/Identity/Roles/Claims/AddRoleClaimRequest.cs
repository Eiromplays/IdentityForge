namespace Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

public class AddRoleClaimRequest
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}

public class AddRoleClaimRequestValidator : Validator<AddRoleClaimRequest>
{
    public AddRoleClaimRequestValidator()
    {
        RuleFor(uc => uc.Type)
            .NotEmpty();

        RuleFor(uc => uc.Value)
            .NotEmpty();
    }
}