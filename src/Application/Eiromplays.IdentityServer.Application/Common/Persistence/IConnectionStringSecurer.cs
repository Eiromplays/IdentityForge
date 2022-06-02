using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Common.Persistence;

public interface IConnectionStringSecurer
{
    string? MakeSecure(string? connectionString, DatabaseProvider? dbProvider = null);
}