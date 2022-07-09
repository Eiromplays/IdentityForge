namespace Eiromplays.IdentityServer.Application.Identity.Sessions.Requests;

public class DeleteServerSideSessionRequest
{
    public string SessionId { get; set; } = default!;
}

public class DeleteServerSideSessionRequestValidator : Validator<DeleteServerSideSessionRequest>
{
    public DeleteServerSideSessionRequestValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
    }
}