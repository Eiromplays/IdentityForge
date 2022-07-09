using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.UpdateProfile;

public class Endpoint : Endpoint<UpdateProfileRequest, UpdateProfileResponse>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/personal/profile");
        Summary(s =>
        {
            s.Summary = "Update profile details of currently logged in user.";
        });
        Version(1);
        ScopedValidator();
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response = await _userService.UpdateAsync(req, userId, BaseURL, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}