namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public interface IClientService : ITransientService
{
    Task<PaginationResponse<Duende.IdentityServer.Models.Client>> SearchAsync(ClientListFilter filter,
        CancellationToken cancellationToken = default);
    
    Task<ClientDto> GetAsync(string? clientId, CancellationToken cancellationToken = default);
}