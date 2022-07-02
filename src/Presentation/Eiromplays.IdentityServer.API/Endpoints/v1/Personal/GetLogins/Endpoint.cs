using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetLogins;

public class Endpoint : EndpointWithoutRequest<ExternalLoginsResponse>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/personal/external-logins");
        Summary(s =>
        {
            s.Summary = "Get logins of currently logged in user.";
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

        Response = await _userService.GetExternalLoginsAsync(userId);

        await SendAsync(Response, cancellation: ct);
    }
}