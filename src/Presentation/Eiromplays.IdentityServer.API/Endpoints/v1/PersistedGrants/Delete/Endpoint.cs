using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.Delete;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IPersistedGrantService _persistedGrantService;

    public Endpoint(IPersistedGrantService persistedGrantService)
    {
        _persistedGrantService = persistedGrantService;
    }

    public override void Configure()
    {
        Delete("/persisted-grants/{Key}");
        Summary(s =>
        {
            s.Summary = "Delete a persisted grant.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Delete, EIAResource.PersistedGrants));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _persistedGrantService.DeleteAsync(req.Key, ct);

        return Response;
    }
}