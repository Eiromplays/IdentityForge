using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetPersistedGrantBySubjectId;

public class Endpoint : Endpoint<Models.Request, List<PersistedGrantDto>>
{
    private readonly IPersistedGrantService _persistedGrantService;

    public Endpoint(IPersistedGrantService persistedGrantService)
    {
        _persistedGrantService = persistedGrantService;
    }

    public override void Configure()
    {
        Get("/persisted-grants/subjects/{SubjectId}");
        Summary(s =>
        {
            s.Summary = "Get persisted grants by SubjectId.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.PersistedGrants));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _persistedGrantService.GetUserPersistedGrantsAsync(req.SubjectId, ct);
        
        await SendOkAsync(Response, cancellation: ct);
    }
}