using Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<EIATenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EIATenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}