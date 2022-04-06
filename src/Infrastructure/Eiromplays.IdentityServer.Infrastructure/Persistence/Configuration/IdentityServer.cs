using Duende.Bff.EntityFramework;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Configuration;

public class IdentityServerClientsConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClients, SchemaNames.IdentityServer);
}

public class IdentityServerClientCorsOriginsConfig : IEntityTypeConfiguration<ClientCorsOrigin>
{
    public void Configure(EntityTypeBuilder<ClientCorsOrigin> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientCorsOrigins, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityResourcesConfig : IEntityTypeConfiguration<IdentityResource>
{
    public void Configure(EntityTypeBuilder<IdentityResource> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerIdentityResources, SchemaNames.IdentityServer);
}

public class IdentityServerApiResourcesConfig : IEntityTypeConfiguration<ApiResource>
{
    public void Configure(EntityTypeBuilder<ApiResource> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiResources, SchemaNames.IdentityServer);
}

public class IdentityServerApiScopesConfig : IEntityTypeConfiguration<ApiScope>
{
    public void Configure(EntityTypeBuilder<ApiScope> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiScopes, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityProvidersConfig : IEntityTypeConfiguration<IdentityProvider>
{
    public void Configure(EntityTypeBuilder<IdentityProvider> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerIdentityProviders, SchemaNames.IdentityServer);
}

public class IdentityServerApiResourceClaimsConfig : IEntityTypeConfiguration<ApiResourceClaim>
{
    public void Configure(EntityTypeBuilder<ApiResourceClaim> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiResourceClaims, SchemaNames.IdentityServer);
}

public class IdentityServerApiResourcePropertyConfig : IEntityTypeConfiguration<ApiResourceProperty>
{
    public void Configure(EntityTypeBuilder<ApiResourceProperty> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiResourceProperties, SchemaNames.IdentityServer);
}

public class IdentityServerApiResourceScopesConfig : IEntityTypeConfiguration<ApiResourceScope>
{
    public void Configure(EntityTypeBuilder<ApiResourceScope> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiResourceScopes, SchemaNames.IdentityServer);
}

public class IdentityServerApiResourceSecretsConfig : IEntityTypeConfiguration<ApiResourceSecret>
{
    public void Configure(EntityTypeBuilder<ApiResourceSecret> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiResourceSecrets, SchemaNames.IdentityServer);
}

public class IdentityServerApiScopeClaimsConfig : IEntityTypeConfiguration<ApiScopeClaim>
{
    public void Configure(EntityTypeBuilder<ApiScopeClaim> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiScopeClaims, SchemaNames.IdentityServer);
}

public class IdentityServerApiScopePropertyConfig : IEntityTypeConfiguration<ApiScopeProperty>
{
    public void Configure(EntityTypeBuilder<ApiScopeProperty> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerApiScopeProperties, SchemaNames.IdentityServer);
}

public class IdentityServerClientClaimsConfig : IEntityTypeConfiguration<ClientClaim>
{
    public void Configure(EntityTypeBuilder<ClientClaim> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientClaims, SchemaNames.IdentityServer);
}

public class IdentityServerClientGrantTypesConfig : IEntityTypeConfiguration<ClientGrantType>
{
    public void Configure(EntityTypeBuilder<ClientGrantType> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientGrantTypes, SchemaNames.IdentityServer);
}

public class IdentityServerClientIdPRestrictionsConfig : IEntityTypeConfiguration<ClientIdPRestriction>
{
    public void Configure(EntityTypeBuilder<ClientIdPRestriction> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientIdPRestrictions, SchemaNames.IdentityServer);
}

public class IdentityServerClientPostLogoutRedirectUrisConfig : IEntityTypeConfiguration<ClientPostLogoutRedirectUri>
{
    public void Configure(EntityTypeBuilder<ClientPostLogoutRedirectUri> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientPostLogoutRedirectUris, SchemaNames.IdentityServer);
}

public class IdentityServerClientScopesConfig : IEntityTypeConfiguration<ClientScope>
{
    public void Configure(EntityTypeBuilder<ClientScope> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientScopes, SchemaNames.IdentityServer);
}

public class IdentityServerClientSecretsConfig : IEntityTypeConfiguration<ClientSecret>
{
    public void Configure(EntityTypeBuilder<ClientSecret> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientSecrets, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityResourceClaimsConfig : IEntityTypeConfiguration<IdentityResourceClaim>
{
    public void Configure(EntityTypeBuilder<IdentityResourceClaim> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerIdentityResourceClaims, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityResourcePropertyConfig : IEntityTypeConfiguration<IdentityResourceProperty>
{
    public void Configure(EntityTypeBuilder<IdentityResourceProperty> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerIdentityResourceProperties, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityClientPropertyConfig : IEntityTypeConfiguration<ClientProperty>
{
    public void Configure(EntityTypeBuilder<ClientProperty> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientProperties, SchemaNames.IdentityServer);
}

public class IdentityServerIdentityClientRedirectUrisConfig : IEntityTypeConfiguration<ClientRedirectUri>
{
    public void Configure(EntityTypeBuilder<ClientRedirectUri> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerClientRedirectUris, SchemaNames.IdentityServer);
}

public class IdentityServerPersistedGrantsConfig : IEntityTypeConfiguration<PersistedGrant>
{
    public void Configure(EntityTypeBuilder<PersistedGrant> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerPersistedGrants, SchemaNames.IdentityServer)
            .HasKey(x => x.Key);
}

public class IdentityServerDeviceFlowCodesConfig : IEntityTypeConfiguration<DeviceFlowCodes>
{
    public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerDeviceFlowCodes, SchemaNames.IdentityServer)
            .HasKey(x => x.UserCode);
}

public class IdentityServerKeysConfig : IEntityTypeConfiguration<Key>
{
    public void Configure(EntityTypeBuilder<Key> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerKeys, SchemaNames.IdentityServer);
}

public class IdentityServerServerSideSessionConfig : IEntityTypeConfiguration<ServerSideSession>
{
    public void Configure(EntityTypeBuilder<ServerSideSession> builder) =>
        builder
            .ToTable(TableConsts.IdentityServerServerSideSessions, SchemaNames.IdentityServer);
}