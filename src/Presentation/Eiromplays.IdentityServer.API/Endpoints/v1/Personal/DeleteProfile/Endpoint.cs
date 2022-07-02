using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.DeleteProfile;

public class Endpoint : EndpointWithoutRequest
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/personal/profile");
        Summary(s =>
        {
            s.Summary = "Delete currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await _userService.DeleteAsync(userId);

        await SendNoContentAsync(cancellation: ct);
    }
}