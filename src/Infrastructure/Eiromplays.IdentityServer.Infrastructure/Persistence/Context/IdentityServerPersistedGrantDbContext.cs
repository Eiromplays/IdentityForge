﻿using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Multitenancy.Context;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context;

public class IdentityServerPersistedGrantDbContext : BaseIdentityServerPersistedGrantDbContext<IdentityServerPersistedGrantDbContext>
{
    public IdentityServerPersistedGrantDbContext(ITenantInfo currentTenant,
        DbContextOptions<IdentityServerPersistedGrantDbContext> options, ICurrentUser currentUser,
        ISerializerService serializer, IOptions<DatabaseConfiguration> databaseConfiguration,
        IEventPublisher events, IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext)
        : base(currentTenant, options, currentUser, serializer, databaseConfiguration, events, webHostEnvironment, applicationDbContext)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.IdentityServer);
    }
}