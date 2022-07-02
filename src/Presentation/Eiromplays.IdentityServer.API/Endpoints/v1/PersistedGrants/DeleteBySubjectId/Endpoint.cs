using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.DeleteBySubjectId;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IPersistedGrantService _persistedGrantService;

    public Endpoint(IPersistedGrantService persistedGrantService)
    {
        _persistedGrantService = persistedGrantService;
    }

    public override void Configure()
    {
        Delete("/persisted-grants/subjects/{SubjectId}");
        Summary(s =>
        {
            s.Summary = "Delete all persisted grants for a user.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.PersistedGrants));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _persistedGrantService.DeleteUserPersistedGrantsAsync(req.SubjectId, ct);

        await SendAsync(Response, cancellation: ct);
    }
}