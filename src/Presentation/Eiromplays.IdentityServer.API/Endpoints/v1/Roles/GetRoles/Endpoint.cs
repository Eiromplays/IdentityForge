using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRoles;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private IIdentityService IdentityService { get; }
    
    public Endpoint(IIdentityService identityService)
    {
        IdentityService = identityService;
    }
    
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/roles");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Roles = await IdentityService.GetRolesAsync(req.Search, req.PageIndex, req.PageSize);
        
        await SendAsync(Response, cancellation: ct);
    }
}