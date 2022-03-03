using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.DTOs.User;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUser;

public class Endpoint : Endpoint<Models.Request, UserDto>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/users/{Id}");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
        {
            //ThrowError($"User with id {req.Id} not found");
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(user, cancellation: ct);
    }
}