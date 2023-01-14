using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class UpdateProfileEndpoint : Endpoint<UpdateProfileRequest, UpdateProfileResponse>
{
    private readonly IUserService _userService;

    public UpdateProfileEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/manage/update-profile");
        Summary(s =>
        {
            s.Summary = "Update profile details of currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response = await _userService.UpdateProfileAsync(req, userId, BaseURL, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}