namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class IsAuthenticatedEndpoint : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Get("/account/is-authenticated");
        Summary(s =>
        {
            s.Summary = "Check if the user is authenticated";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(User.Identity?.IsAuthenticated ?? false, cancellation: ct);
    }
}