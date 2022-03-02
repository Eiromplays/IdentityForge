using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private IIdentityService IdentityService { get; }
    
    public Endpoint(IIdentityService identityService)
    {
        IdentityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/users");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var (result, userId) = await IdentityService.CreateUserAsync(req.UserDto);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();
        
        if (userId is null)
            ThrowError("User was not created");
        
        await SendCreatedAtAsync("/users/{id}", userId, new Models.Response{UserDto = req.UserDto}, ct);
    }
}