using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.CreateClient;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IClientService _clientService;
    
    public Endpoint(IClientService clientService)
    {
        _clientService = clientService;
    }

    public override void Configure()
    {
        Post("/clients");
        Summary(s =>
        {
            s.Summary = "Creates a new client.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.Clients));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        // TODO: Add a option to allow anonymous users to create users
        // Returns Unauthorized if it is disabled
        // TODO: Add some more protection, like a captcha or something
        Response.Message = await _clientService.CreateAsync(req.Data, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}