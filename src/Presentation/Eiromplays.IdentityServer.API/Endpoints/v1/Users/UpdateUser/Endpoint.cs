using Eiromplays.IdentityServer.Application.Identity.Users;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/users/{id}");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (!User.IsInRole("Administrator") && (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await _userService.UpdateAsync(req.UpdateUserRequest, req.Id);

        await SendOkAsync(ct);
    }
}