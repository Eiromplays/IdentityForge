namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public interface IClientService : ITransientService
{
    Task<PaginationResponse<ClientDto>> SearchAsync(ClientListFilter filter,
        CancellationToken cancellationToken = default);
    
    Task<ClientDto> GetAsync(int clientId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateClientRequest request, int clientId, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(int clientId, CancellationToken cancellationToke = default);
    
    Task<string> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsWithClientIdAsync(string clientId, CancellationToken cancellationToken = default, int? exceptId = null);
}