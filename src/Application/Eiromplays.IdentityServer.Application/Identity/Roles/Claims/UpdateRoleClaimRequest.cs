namespace Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

public class UpdateRoleClaimRequest
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}

public class UpdateRoleClaimRequestValidator : Validator<UpdateRoleClaimRequest>
{
    public UpdateRoleClaimRequestValidator()
    {
        RuleFor(uc => uc.Type)
            .NotEmpty();

        RuleFor(uc => uc.Value)
            .NotEmpty();
    }
}