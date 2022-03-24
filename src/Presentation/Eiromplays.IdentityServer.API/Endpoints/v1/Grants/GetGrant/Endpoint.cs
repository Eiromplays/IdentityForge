using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Grants.GetGrant;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
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