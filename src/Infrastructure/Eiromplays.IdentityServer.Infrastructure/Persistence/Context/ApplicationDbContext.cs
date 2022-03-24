using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext, IDataProtectionKeyContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser,
        ISerializerService serializer, IOptionsMonitor<DatabaseConfiguration> databaseConfiguration,
        IEventPublisher events, IWebHostEnvironment webHostEnvironment)
        : base(currentTenant, options, currentUser, serializer, databaseConfiguration, events, webHostEnvironment)
    {
    }


    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}