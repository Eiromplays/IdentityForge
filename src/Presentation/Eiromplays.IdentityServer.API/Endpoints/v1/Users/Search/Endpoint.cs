using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Search;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }
    
    public override void Configure()
    {
        Post("/users/search");
        Summary(s =>
        {
            s.Summary = "Search users using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.Users));
    }
    
    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Users = await _userService.SearchAsync(request.UserListFilter, ct);

        return Response;
    }
}