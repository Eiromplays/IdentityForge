using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Persistence;
using Eiromplays.IdentityServer.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.ConnectionString;

public class ConnectionStringSecurer : IConnectionStringSecurer
{
    private const string HiddenValueDefault = "*******";
    private readonly DatabaseConfiguration _databaseConfiguration;

    public ConnectionStringSecurer(IOptions<DatabaseConfiguration> databaseConfiguration) =>
        _databaseConfiguration = databaseConfiguration.Value;

    public string? MakeSecure(string? connectionString, DatabaseProvider? dbProvider)
    {
        if (connectionString is null || string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        dbProvider ??= _databaseConfiguration.DatabaseProvider;

        return dbProvider switch
        {
            DatabaseProvider.PostgreSql => MakeSecureNpgsqlConnectionString(connectionString),
            DatabaseProvider.SqlServer => MakeSecureSqlConnectionString(connectionString),
            DatabaseProvider.MySql => MakeSecureMySqlConnectionString(connectionString),
            _ => connectionString
        };
    }

    private string MakeSecureMySqlConnectionString(string connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password))
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.UserID))
        {
            builder.UserID = HiddenValueDefault;
        }

        return builder.ToString();
    }

    private string MakeSecureSqlConnectionString(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
        {
            builder.UserID = HiddenValueDefault;
        }

        return builder.ToString();
    }

    private string MakeSecureNpgsqlConnectionString(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.Username) || !builder.IntegratedSecurity)
        {
            builder.Username = HiddenValueDefault;
        }

        return builder.ToString();
    }
}