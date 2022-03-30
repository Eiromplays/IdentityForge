using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Multitenancy.Context;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context;

public class IdentityServerConfigurationDbContext : BaseIdentityServerConfigurationDbContext<IdentityServerConfigurationDbContext>
{
    public IdentityServerConfigurationDbContext(
        DbContextOptions<IdentityServerConfigurationDbContext> options, ICurrentUser currentUser,
        ISerializerService serializer, IOptions<DatabaseConfiguration> databaseConfiguration,
        IEventPublisher events, IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext)
        : base(options, currentUser, serializer, databaseConfiguration, events, webHostEnvironment, applicationDbContext)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.IdentityServer);
    }
}