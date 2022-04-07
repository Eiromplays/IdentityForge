using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

public class Endpoint : EndpointWithoutRequest<List<UserDetailsDto>>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }
    
    public override void Configure()
    {
        Get("/users");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get list of all users.";
        });
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Users));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _userService.GetListAsync(ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}