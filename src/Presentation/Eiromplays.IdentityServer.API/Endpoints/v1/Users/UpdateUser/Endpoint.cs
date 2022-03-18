using System.Net;
using Duende.Bff.EntityFramework;
using Duende.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

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
        if (!User.IsInRole("Administrator") && !User.HasClaim("sub", req.Id))
        {
            AddError($"You do not have permissions to update {req.Id}'s Profile");;
            await SendErrorsAsync((int)HttpStatusCode.Unauthorized, ct);
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