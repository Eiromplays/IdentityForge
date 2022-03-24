using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context
{
    public class IdentityServerConfigurationDbContext : ConfigurationDbContext<IdentityServerConfigurationDbContext>
    {
        public IdentityServerConfigurationDbContext(DbContextOptions<IdentityServerConfigurationDbContext> options) : base(options)
        {
            
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
    }
}