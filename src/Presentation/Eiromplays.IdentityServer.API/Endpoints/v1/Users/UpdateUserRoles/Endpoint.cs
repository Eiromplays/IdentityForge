using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUserRoles;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a user's assigned roles.";
        });
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var response = await _userService.AssignRolesAsync(req.Id, req, ct);

        await SendAsync(new Models.Response{ Message = response }, cancellation: ct);
    }
}