namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetById;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    public override void Configure()
    {
        Get("/grants/{Id}");
        Summary(s =>
        {
            s.Summary = "Get persisted grants by Id.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.PersistedGrants));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        
    }
}