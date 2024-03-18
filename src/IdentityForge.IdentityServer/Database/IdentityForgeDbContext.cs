using System.Linq.Expressions;
using Duende.Bff.EntityFramework;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Interfaces;
using IdentityForge.IdentityServer.Domain;
using IdentityForge.IdentityServer.Domain.Roles;
using IdentityForge.IdentityServer.Domain.Users;
using IdentityForge.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IdentityForge.IdentityServer.Database;

public class IdentityForgeDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>,
    ISessionDbContext, IDataProtectionKeyContext, IPersistedGrantDbContext, IConfigurationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public IdentityForgeDbContext(
        DbContextOptions<IdentityForgeDbContext> options,
        ICurrentUserService currentUserService,
        IMediator mediator,
        IDateTimeProvider dateTimeProvider) : base(options)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
    }

    #region DbSet Region - Do Not Delete
    public DbSet<PersistedGrant> PersistedGrants { get; set; } = null!;
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; } = null!;
    public DbSet<Key> Keys { get; set; } = null!;
    public DbSet<ServerSideSession> ServerSideSessions { get; set; } = null!;

    public DbSet<UserSessionEntity> UserSessions { get; set; } = null!;

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; } = null!;
    public DbSet<IdentityResource> IdentityResources { get; set; } = null!;
    public DbSet<ApiResource> ApiResources { get; set; } = null!;
    public DbSet<ApiScope> ApiScopes { get; set; } = null!;
    public DbSet<IdentityProvider> IdentityProviders { get; set; } = null!;
    #endregion DbSet Region - Do Not Delete

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.FilterSoftDeletedRecords();
        /* any query filters added after this will override soft delete
            https://docs.microsoft.com/en-us/ef/core/querying/filters
            https://github.com/dotnet/efcore/issues/10275
        */

        #region Entity Database Config Region - Only delete if you don't want to automatically add configurations
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        #endregion Entity Database Config Region - Only delete if you don't want to automatically add configurations
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        int result = base.SaveChanges();
        _dispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        int result = await base.SaveChangesAsync(cancellationToken);
        await _dispatchDomainEvents();
        return result;
    }

    private async Task _dispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<IBaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }

    private void UpdateAuditFields()
    {
        var now = _dateTimeProvider.DateTimeUtcNow;
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UpdateCreationProperties(now, _currentUserService.UserId);
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService.UserId);
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService.UserId);
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService.UserId);
                    entry.Entity.UpdateIsDeleted(true);
                    break;
            }
        }
    }
}

public static class Extensions
{
    public static void FilterSoftDeletedRecords(this ModelBuilder modelBuilder)
    {
        Expression<Func<BaseEntity, bool>> filterExpr = e => !e.IsDeleted;
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes()
                     .Where(m => m.ClrType.IsAssignableTo(typeof(BaseEntity))))
        {
            // modify expression to handle correct child type
            var parameter = Expression.Parameter(mutableEntityType.ClrType);
            var body = ReplacingExpressionVisitor
                .Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
            var lambdaExpression = Expression.Lambda(body, parameter);

            // set filter
            mutableEntityType.SetQueryFilter(lambdaExpression);
        }
    }
}
