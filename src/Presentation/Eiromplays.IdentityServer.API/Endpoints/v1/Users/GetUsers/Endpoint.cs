using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

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
        Routes("/users");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Users = await _identityService.GetUsersAsync(req.Search, req.PageIndex, req.PageSize, ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}