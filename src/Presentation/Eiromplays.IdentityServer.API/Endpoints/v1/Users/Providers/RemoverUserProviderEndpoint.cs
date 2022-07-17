using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Providers;

public class RemoverUserProviderEndpoint : Endpoint<RemoveUserProviderModels.Request, RemoveUserProviderModels.Response>
{
    private readonly IUserService _userService;

    public RemoverUserProviderEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/{Id}/providers-delete");
        Summary(s =>
        {
            s.Summary = "Remove a user provider";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(RemoveUserProviderModels.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.RemoveLoginAsync(req.RemoveLoginRequest, req.Id);

        await SendOkAsync(Response, cancellation: ct);
    }
}