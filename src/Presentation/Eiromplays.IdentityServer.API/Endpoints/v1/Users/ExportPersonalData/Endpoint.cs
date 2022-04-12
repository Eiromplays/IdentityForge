using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ExportPersonalData;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}/export-personal-data");
        Summary(s =>
        {
            s.Summary = "Export data for user.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var result = await _userService.ExportPersonalDataAsync(req.Id);

        await SendStreamAsync(result, "PersonalData.csv", result.Length, contentType: "application/octet-stream",
            cancellation: ct);
    }
}