using Duende.Bff.EntityFramework;
using Eiromplays.IdentityServer.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;

public class BffUserSessionsConfig : IEntityTypeConfiguration<UserSessionEntity>
{
    public void Configure(EntityTypeBuilder<UserSessionEntity> builder) =>
        builder
            .ToTable(TableConsts.BffUserSessions, SchemaNames.Bff);
}