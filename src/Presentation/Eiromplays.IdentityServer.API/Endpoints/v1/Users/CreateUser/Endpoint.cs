using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
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
        // TODO: Add a option to allow anonymous users to create users
        // Returns Unauthorized if it is disabled
        // TODO: Add some more protection, like a captcha or something
        var userId = await _userService.CreateAsync(req.UserDto, BaseURL);

        await SendCreatedAtAsync<GetUserById.Endpoint>(new GetUserById.Models.Request { Id = userId },
            new Models.Response { UserId = userId }, cancellation: ct);
    }
}