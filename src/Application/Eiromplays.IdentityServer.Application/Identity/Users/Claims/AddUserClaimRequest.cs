namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class AddUserClaimRequest
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}

public class AddUserClaimRequestValidator : Validator<AddUserClaimRequest>
{
    public AddUserClaimRequestValidator()
    {
        RuleFor(uc => uc.Type)
            .NotEmpty();

        RuleFor(uc => uc.Value)
            .NotEmpty();
    }
}