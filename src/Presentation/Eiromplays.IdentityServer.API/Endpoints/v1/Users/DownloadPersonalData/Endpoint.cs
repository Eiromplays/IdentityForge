using System.Text.Json;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DownloadPersonalData;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}/download-personal-data");
        Summary(s =>
        {
            s.Summary = "Get personal data for user.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var user = await _userService.GetAsync(req.Id, ct);

        var personalData = await _userService.GetPersonalDataAsync(user.Id);

        await SendBytesAsync(JsonSerializer.SerializeToUtf8Bytes(personalData, new JsonSerializerOptions{WriteIndented = true}), "PersonalData.json",
            cancellation: ct);
    }
}