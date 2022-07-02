using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUser currentUser,
        ISerializerService serializer,
        IOptions<DatabaseConfiguration> databaseConfiguration,
        IEventPublisher events,
        IWebHostEnvironment webHostEnvironment)
        : base(options, currentUser, serializer, databaseConfiguration, events, webHostEnvironment)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}