using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;

public class IdentityServerClientConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClients, SchemaNames.IdentityServer);
}

public class IdentityServerClientCorsOriginConfig : IEntityTypeConfiguration<ClientCorsOrigin>
{
    public void Configure(EntityTypeBuilder<ClientCorsOrigin> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientCorsOrigins, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityResourceConfig : IEntityTypeConfiguration<IdentityResource>
{
    public void Configure(EntityTypeBuilder<IdentityResource> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerIdentityResources, SchemaNames.IdentityServer);
}

public class IdentityServerApiResourceConfig : IEntityTypeConfiguration<ApiResource>
{
    public void Configure(EntityTypeBuilder<ApiResource> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiResources, SchemaNames.IdentityServer);
}

public class IdentityServerApiScopeConfig : IEntityTypeConfiguration<ApiScope>
{
    public void Configure(EntityTypeBuilder<ApiScope> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiScopes, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityProviderConfig : IEntityTypeConfiguration<IdentityProvider>
{
    public void Configure(EntityTypeBuilder<IdentityProvider> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerIdentityProviders, SchemaNames.IdentityServer);
}

/*public class IdentityServerPersistedGrantConfig : IEntityTypeConfiguration<PersistedGrant>
{
    public void Configure(EntityTypeBuilder<PersistedGrant> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerPersistedGrants, SchemaNames.IdentityServer)
            .IsMultiTenant();
}

public class IdentityServerDeviceFlowCodesConfig : IEntityTypeConfiguration<DeviceFlowCodes>
{
    public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerDeviceFlowCodes, SchemaNames.IdentityServer)
            .IsMultiTenant();
}*/