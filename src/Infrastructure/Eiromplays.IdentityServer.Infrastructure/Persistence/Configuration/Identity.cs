using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .ToTable(TableConsts.IdentityUsers, SchemaNames.Identity);

        builder
            .Property(u => u.ObjectId)
                .HasMaxLength(256);
    }
}

public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder) =>
        builder
            .ToTable(TableConsts.IdentityRoles, SchemaNames.Identity);
}

public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder) =>
        builder
            .ToTable(TableConsts.IdentityRoleClaims, SchemaNames.Identity);
}

public class ApplicationIdentityUserRoleConfig : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder) =>
        builder
            .ToTable(TableConsts.IdentityUserRoles, SchemaNames.Identity);
}

public class ApplicationIdentityUserClaimConfig : IEntityTypeConfiguration<ApplicationUserClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder) =>
        builder
            .ToTable(TableConsts.IdentityUserClaims, SchemaNames.Identity);
}

public class ApplicationIdentityUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder) =>
        builder
            .ToTable(TableConsts.IdentityUserLogins, SchemaNames.Identity);
}

public class ApplicationIdentityUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder) =>
        builder
            .ToTable(TableConsts.IdentityUserTokens, SchemaNames.Identity);
}

public class DataProtectionKeyConfig : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> builder) =>
        builder
            .ToTable("DataProtectionKeys", SchemaNames.Identity);
}