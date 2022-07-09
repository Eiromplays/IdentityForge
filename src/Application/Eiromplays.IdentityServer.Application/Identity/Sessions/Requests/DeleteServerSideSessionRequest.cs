namespace Eiromplays.IdentityServer.Application.Identity.Sessions.Requests;

public class DeleteServerSideSessionRequest
{
    public string Key { get; set; } = default!;
}

public class DeleteServerSideSessionRequestValidator : Validator<DeleteServerSideSessionRequest>
{
    public DeleteServerSideSessionRequestValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
    }
}