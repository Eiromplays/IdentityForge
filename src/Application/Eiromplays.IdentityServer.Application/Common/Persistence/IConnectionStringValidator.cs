using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Common.Persistence;

public interface IConnectionStringValidator
{
    bool TryValidate(string connectionString, DatabaseProvider? dbProvider = null);
}