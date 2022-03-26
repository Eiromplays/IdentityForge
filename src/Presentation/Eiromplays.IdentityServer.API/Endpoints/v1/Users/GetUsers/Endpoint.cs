using Eiromplays.IdentityServer.Application.Identity.Users;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
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
        Response.Users = await _userService.SearchAsync(req, ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}