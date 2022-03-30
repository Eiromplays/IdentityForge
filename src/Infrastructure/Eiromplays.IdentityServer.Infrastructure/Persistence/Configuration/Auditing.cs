using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;

public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder) =>
        builder
            .ToTable(TableConsts.AuditTrails, SchemaNames.Auditing);
}