using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

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
        Routes("/users");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Users = await IdentityService.GetUsersAsync(req.Search, req.PageIndex, req.PageSize, ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}