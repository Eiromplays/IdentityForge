namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class UpdateUserClaimRequest
{
    public string NewType { get; set; } = default!;
    public string NewValue { get; set; } = default!;

    public string OldType { get; set; } = default!;
    public string OldValue { get; set; } = default!;
}

public class UpdateUserClaimRequestValidator : Validator<UpdateUserClaimRequest>
{
    public UpdateUserClaimRequestValidator()
    {
        RuleFor(uc => uc.NewType)
            .NotEmpty();

        RuleFor(uc => uc.NewValue)
            .NotEmpty();

        RuleFor(uc => uc.OldType)
            .NotEmpty();

        RuleFor(uc => uc.OldValue )
            .NotEmpty();
    }
}