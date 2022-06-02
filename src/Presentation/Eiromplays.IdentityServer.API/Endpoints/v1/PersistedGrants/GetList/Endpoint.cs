using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetList;

public class Endpoint : EndpointWithoutRequest<List<PersistedGrantDto>>
{
    private readonly IPersistedGrantService _persistedGrantService;

    public Endpoint(IPersistedGrantService persistedGrantService)
    {
        _persistedGrantService = persistedGrantService;
    }

    public override void Configure()
    {
        Get("/persisted-grants");
        Summary(s =>
        {
            s.Summary = "Get list of all persisted grants.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.PersistedGrants));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _persistedGrantService.GetListAsync(ct);
        
        await SendOkAsync(Response, cancellation: ct);
    }
}