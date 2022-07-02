using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetProfile;

public class Endpoint : EndpointWithoutRequest<UserDetailsDto>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/personal/profile");
        Summary(s =>
        {
            s.Summary = "Get profile details of currently logged in user.";
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

        Response = await _userService.GetAsync(userId, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}