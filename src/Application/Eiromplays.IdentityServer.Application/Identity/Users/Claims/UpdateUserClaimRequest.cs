namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class UpdateUserClaimRequest
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}

public class UpdateUserClaimRequestValidator : Validator<UpdateUserClaimRequest>
{
    public UpdateUserClaimRequestValidator()
    {
        RuleFor(uc => uc.Type)
            .NotEmpty();

        RuleFor(uc => uc.Value)
            .NotEmpty();
    }
}