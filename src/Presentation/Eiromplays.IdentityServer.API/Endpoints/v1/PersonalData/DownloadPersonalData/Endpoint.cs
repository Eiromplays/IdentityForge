using System.Text.Json;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersonalData.DownloadPersonalData;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/personal-data/{Id}/download");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (!User.IsInRole("Administrator") && !User.HasClaim("sub", req.Id!))
        {
            AddError($"You do not have permissions to download {req.Id}'s personal data");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }
        
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
        {
            AddError($"User with id {req.Id} not found");;
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var personalData = await _identityService.GetUserPersonalDataAsync(user.Id);

        await SendBytesAsync(JsonSerializer.SerializeToUtf8Bytes(personalData), "PersonalData.json",
            cancellation: ct);
    }
}