using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Infrastructure.BackgroundJobs;

public class HangfireStorageSettings
{
    public DatabaseProvider StorageProvider { get; set; }
    public string? ConnectionString { get; set; }
}