using Eiromplays.IdentityServer.Application.Identity.Users;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ToggleUserStatus;

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
        Routes("/users/{Id}/toggle-status");
        Summary(s =>
        {
            s.Summary = "Toggle a user's active status.";
        });
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (!User.IsInRole("Administrator") && !(User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        if (req.Id != req.UserId) 
        {
            AddError("UserId and Id must match.");
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await _userService.ToggleStatusAsync(req, ct);

        await SendOkAsync(cancellation: ct);
    }
}