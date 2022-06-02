using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<ClientDto>>
{
    private readonly IClientService _clientService;
    
    public Endpoint(IClientService clientService)
    {
        _clientService = clientService;
    }

    public override void Configure()
    {
        Post("/clients/search");
        Summary(s =>
        {
            s.Summary = "Search clients using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.Clients));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _clientService.SearchAsync(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}