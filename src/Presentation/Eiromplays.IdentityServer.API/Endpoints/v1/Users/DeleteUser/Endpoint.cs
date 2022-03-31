using Eiromplays.IdentityServer.Application.Identity.Users;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DeleteUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/users/{Id}");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        //var result = await _userService.Delete(req.Id);

        await SendAsync(Response, cancellation: ct);
    }
}