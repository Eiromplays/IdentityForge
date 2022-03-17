using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DeleteUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/users/{Id}");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var result = await _identityService.DeleteUserAsync(req.Id);

        await SendAsync(new Models.Response{ Result = result }, cancellation: ct);
    }
}