using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts;

public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim,
    ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IDomainEventService _domainEventService;

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ICurrentUserService currentUserService,
        IDateTime dateTime, IDomainEventService domainEventService) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _domainEventService = domainEventService;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents();

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        ConfigureIdentityDbContext(builder);
    }

    private async Task DispatchEvents()
    {
        while (true)
        {
            var domainEventEntity = ChangeTracker
                .Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

            if (domainEventEntity == null) break;

            domainEventEntity.IsPublished = true;
            await _domainEventService.Publish(domainEventEntity);
        }
    }

    private static void ConfigureIdentityDbContext(ModelBuilder builder)
    {
        builder.Entity<ApplicationRole>().ToTable(TableConsts.IdentityRoles);
        builder.Entity<ApplicationRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
        builder.Entity<ApplicationUserRole>().ToTable(TableConsts.IdentityUserRoles);

        builder.Entity<ApplicationUser>().ToTable(TableConsts.IdentityUsers);
        builder.Entity<ApplicationUserLogin>().ToTable(TableConsts.IdentityUserLogins);
        builder.Entity<ApplicationUserClaim>().ToTable(TableConsts.IdentityUserClaims);
        builder.Entity<ApplicationUserToken>().ToTable(TableConsts.IdentityUserTokens);
    }
}