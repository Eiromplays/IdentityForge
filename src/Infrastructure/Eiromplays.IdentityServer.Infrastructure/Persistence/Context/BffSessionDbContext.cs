using Duende.Bff.EntityFramework;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context;

public class BffSessionDbContext : SessionDbContext
{
    public BffSessionDbContext(DbContextOptions<SessionDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserSessionEntity>()
            .ToTable(TableConsts.BffUserSessions, SchemaNames.Bff);
    }
}