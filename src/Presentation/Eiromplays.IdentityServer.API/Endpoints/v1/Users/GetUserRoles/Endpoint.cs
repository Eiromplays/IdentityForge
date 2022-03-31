using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserRoles;

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
        Routes("/users/{Id}/roles");
        Summary(s =>
        {
            s.Summary = "Get a user's roles.";
        });
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var userRoles = await _userService.GetRolesAsync(req.Id, ct);

        await SendAsync(new Models.Response{ UserRolesDto = userRoles }, cancellation: ct);
    }
}