namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class RemoveUserClaimRequest
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}

public class RemoveUserClaimRequestValidator : Validator<RemoveUserClaimRequest>
{
    public RemoveUserClaimRequestValidator()
    {
        RuleFor(uc => uc.Type)
            .NotEmpty();

        RuleFor(uc => uc.Value)
            .NotEmpty();
    }
}