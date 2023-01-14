using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.ProfilePicture;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.UpdateProfilePicture;

public class Endpoint : Endpoint<UpdateProfilePictureRequest>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/personal/update-profile-picture");
        Summary(s =>
        {
            s.Summary = "Update profile picture";
        });
        Version(1);
    }

    public override async Task HandleAsync(UpdateProfilePictureRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response = await _userService.UpdateProfilePictureAsync(req, userId, ct);

        await SendAsync(Response, cancellation: ct);
    }
}