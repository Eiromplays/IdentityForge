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

        public DbSet<ApiResourceProperty> ApiResourceProperties => Set<ApiResourceProperty>();

        public DbSet<IdentityResourceProperty> IdentityResourceProperties => Set<IdentityResourceProperty>();

        public DbSet<ApiResourceSecret> ApiSecrets => Set<ApiResourceSecret>();

        public DbSet<ApiScopeClaim> ApiScopeClaims => Set<ApiScopeClaim>();

        public DbSet<IdentityResourceClaim> IdentityClaims => Set<IdentityResourceClaim>();

        public DbSet<ApiResourceClaim> ApiResourceClaims => Set<ApiResourceClaim>();

        public DbSet<ClientGrantType> ClientGrantTypes => Set<ClientGrantType>();

        public DbSet<ClientScope> ClientScopes => Set<ClientScope>();

        public DbSet<ClientSecret> ClientSecrets => Set<ClientSecret>();

        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris => Set<ClientPostLogoutRedirectUri>();

        public DbSet<ClientIdPRestriction> ClientIdPRestrictions => Set<ClientIdPRestriction>();

        public DbSet<ClientRedirectUri> ClientRedirectUris => Set<ClientRedirectUri>();

        public DbSet<ClientClaim> ClientClaims => Set<ClientClaim>();

        public DbSet<ClientProperty> ClientProperties => Set<ClientProperty>();

        public DbSet<ApiScopeProperty> ApiScopeProperties => Set<ApiScopeProperty>();

        public DbSet<ApiResourceScope> ApiResourceScopes => Set<ApiResourceScope>();

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