using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Grants.GetGrant;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    public Endpoint()
    {
        
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/grants/{Id}");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        
    }
}