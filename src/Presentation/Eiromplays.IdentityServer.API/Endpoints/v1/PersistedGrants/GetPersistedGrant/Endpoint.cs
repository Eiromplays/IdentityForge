using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetPersistedGrant;

public class Endpoint : Endpoint<Models.Request, PersistedGrantDto>
{
    private readonly IPersistedGrantService _persistedGrantService;

    public Endpoint(IPersistedGrantService persistedGrantService)
    {
        _persistedGrantService = persistedGrantService;
    }

    public override void Configure()
    {
        Get("/persisted-grants/{Key}");
        Summary(s =>
        {
            s.Summary = "Get persisted grant by Key.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.PersistedGrants));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _persistedGrantService.GetAsync(req.Key, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}