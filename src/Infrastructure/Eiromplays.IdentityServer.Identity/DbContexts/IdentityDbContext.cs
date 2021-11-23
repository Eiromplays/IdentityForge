using System.Reflection;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Identity.DTOs;
using Eiromplays.IdentityServer.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Configurations;
using EntityFrameworkCore.EncryptColumn;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Eiromplays.IdentityServer.Identity.DbContexts
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, UserTokenDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;
        private readonly IEncryptionProvider _encryptionProvider;

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ICurrentUserService currentUserService,
            IDateTime dateTime, IDomainEventService domainEventService, IConfiguration configuration) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _domainEventService = domainEventService;

            var encryptionKeysConfiguration = configuration.GetSection(nameof(EncryptionKeysConfiguration))
                .Get<EncryptionKeysConfiguration>();

            Initialize.EncryptionKey = encryptionKeysConfiguration.IdentityEncryptionKey;
            _encryptionProvider = new GenerateEncryptionProvider();
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
            builder.Entity<RoleClaimDto>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<UserRoleDto>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<ApplicationUser>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<UserLoginDto>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<UserClaimDto>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<UserTokenDto>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}