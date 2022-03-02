using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private IIdentityService IdentityService { get; }
    
    public Endpoint(IIdentityService identityService)
    {
        IdentityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/users/{id}");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var user = await IdentityService.FindUserByIdAsync(req.Id);
        if (user is null)
            ThrowError("User not found");
        
        user.Email = req.Email;
        user.UserName = req.UserName;
        
        var (result, userId) = await IdentityService.UpdateUserAsync(user);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();
        
        if (user.Id is null)
            ThrowError("User was not updated");
        
        await SendCreatedAtAsync("/users/{id}", user.Id, new Models.Response{UserDto = user}, ct);
    }
}