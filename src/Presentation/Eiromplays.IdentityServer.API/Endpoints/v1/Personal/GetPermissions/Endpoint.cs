using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Infrastructure.Auth.Permissions;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetPermissions;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/personal/permissions");
        Summary(s =>
        {
            s.Summary = "Get permissions of currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response.Permissions = await _userService.GetPermissionsAsync(userId, ct);
        
        await SendOkAsync(Response, cancellation: ct);
    }
}