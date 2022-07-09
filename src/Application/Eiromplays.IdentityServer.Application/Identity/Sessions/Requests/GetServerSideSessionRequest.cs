namespace Eiromplays.IdentityServer.Application.Identity.Sessions.Requests;

public class GetServerSideSessionRequest
{
    public string Key { get; set; } = default!;
}

public class GetServerSideSessionRequestValidator : Validator<GetServerSideSessionRequest>
{
    public GetServerSideSessionRequestValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
    }
}