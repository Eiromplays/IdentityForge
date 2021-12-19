using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts
{
    public class IdentityServerConfigurationDbContext : ConfigurationDbContext<IdentityServerConfigurationDbContext>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public IdentityServerConfigurationDbContext(DbContextOptions<IdentityServerConfigurationDbContext> options,
            ICurrentUserService currentUserService, IDateTime dateTime,
            IDomainEventService domainEventService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _domainEventService = domainEventService;
        }

        public DbSet<ApiResourceProperty>? ApiResourceProperties { get; set; }

        public DbSet<IdentityResourceProperty>? IdentityResourceProperties { get; set; }

        public DbSet<ApiResourceSecret>? ApiSecrets { get; set; }

        public DbSet<ApiScopeClaim>? ApiScopeClaims { get; set; }

        public DbSet<IdentityResourceClaim>? IdentityClaims { get; set; }

        public DbSet<ApiResourceClaim>? ApiResourceClaims { get; set; }

        public DbSet<ClientGrantType>? ClientGrantTypes { get; set; }

        public DbSet<ClientScope>? ClientScopes { get; set; }

        public DbSet<ClientSecret>? ClientSecrets { get; set; }

        public DbSet<ClientPostLogoutRedirectUri>? ClientPostLogoutRedirectUris { get; set; }

        public DbSet<ClientIdPRestriction>? ClientIdPRestrictions { get; set; }

        public DbSet<ClientRedirectUri>? ClientRedirectUris { get; set; }

        public DbSet<ClientClaim>? ClientClaims { get; set; }

        public DbSet<ClientProperty>? ClientProperties { get; set; }

        public DbSet<ApiScopeProperty>? ApiScopeProperties { get; set; }

        public DbSet<ApiResourceScope>? ApiResourceScopes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
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
    }
}