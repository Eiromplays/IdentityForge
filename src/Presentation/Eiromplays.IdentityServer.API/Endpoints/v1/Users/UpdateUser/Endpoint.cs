using System.Net;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;

    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/users/{id}/{RevokeUserSessions?}");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Id))
            ThrowError("Id is required");
        
        if (!User.IsInRole("Administrator") && !User.HasClaim("sub", req.Id!))
        {
            AddError($"You do not have permissions to update {req.Id}'s Profile");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }
        
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
        {
            AddError($"User with id {req.Id} not found");
            await SendErrorsAsync((int)HttpStatusCode.NotFound, ct);
            return;
        }

        user.Email = req.Email;
        user.GravatarEmail = req.GravatarEmail;
        user.UserName = req.UserName;

        var (result, userId) = await _identityService.UpdateUserAsync(user, req.RevokeUserSessions);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();

        await SendCreatedAtAsync("/users/{id}", userId, new Models.Response{UserDto = user}, cancellation: ct);
    }
}