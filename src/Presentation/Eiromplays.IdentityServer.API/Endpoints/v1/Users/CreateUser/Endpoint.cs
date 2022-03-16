using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/users");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var (result, userId) = await _identityService.CreateUserAsync(req.UserDto);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();
        
        if (userId is null)
            ThrowError("User was not created");
        
        await SendCreatedAtAsync("/users/{id}", userId, new Models.Response{UserDto = req.UserDto}, cancellation: ct);
    }
}