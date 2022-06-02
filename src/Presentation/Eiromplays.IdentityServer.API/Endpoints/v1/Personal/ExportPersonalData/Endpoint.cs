using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.ExportPersonalData;

public class Endpoint : EndpointWithoutRequest
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/personal/export-personal-data");
        Summary(s =>
        {
            s.Summary = "Export data for currently logged in user.";
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
        
        var result = await _userService.ExportPersonalDataAsync(userId);

        await SendStreamAsync(result, "PersonalData.csv", result.Length, contentType: "application/octet-stream",
            cancellation: ct);
    }
}