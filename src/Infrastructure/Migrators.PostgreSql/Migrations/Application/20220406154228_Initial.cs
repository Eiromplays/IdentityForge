using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Migrators.PostgreSql.Migrations.Application;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "IdentityServer");

        migrationBuilder.EnsureSchema(
            name: "Auditing");

        migrationBuilder.EnsureSchema(
            name: "Catalog");

        migrationBuilder.EnsureSchema(
            name: "Identity");

        migrationBuilder.CreateTable(
            name: "ApiResources",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>(type: "boolean", nullable: false),
                Name = table.Column<string>(type: "text", nullable: true),
                DisplayName = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "text", nullable: true),
                ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
                RequireResourceIndicator = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                LastAccessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                NonEditable = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ApiScopes",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>(type: "boolean", nullable: false),
                Name = table.Column<string>(type: "text", nullable: true),
                DisplayName = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                Required = table.Column<bool>(type: "boolean", nullable: false),
                Emphasize = table.Column<bool>(type: "boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                LastAccessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                NonEditable = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AuditTrails",
            schema: "Auditing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<string>(type: "text", nullable: true),
                Type = table.Column<string>(type: "text", nullable: true),
                TableName = table.Column<string>(type: "text", nullable: true),
                DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                OldValues = table.Column<string>(type: "text", nullable: true),
                NewValues = table.Column<string>(type: "text", nullable: true),
                AffectedColumns = table.Column<string>(type: "text", nullable: true),
                PrimaryKey = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuditTrails", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Brands",
            schema: "Catalog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                CreatedBy = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                DeletedBy = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Brands", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Clients",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>(type: "boolean", nullable: false),
                ClientId = table.Column<string>(type: "text", nullable: true),
                ProtocolType = table.Column<string>(type: "text", nullable: true),
                RequireClientSecret = table.Column<bool>(type: "boolean", nullable: false),
                ClientName = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                ClientUri = table.Column<string>(type: "text", nullable: true),
                LogoUri = table.Column<string>(type: "text", nullable: true),
                RequireConsent = table.Column<bool>(type: "boolean", nullable: false),
                AllowRememberConsent = table.Column<bool>(type: "boolean", nullable: false),
                AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "boolean", nullable: false),
                RequirePkce = table.Column<bool>(type: "boolean", nullable: false),
                AllowPlainTextPkce = table.Column<bool>(type: "boolean", nullable: false),
                RequireRequestObject = table.Column<bool>(type: "boolean", nullable: false),
                AllowAccessTokensViaBrowser = table.Column<bool>(type: "boolean", nullable: false),
                FrontChannelLogoutUri = table.Column<string>(type: "text", nullable: true),
                FrontChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
                BackChannelLogoutUri = table.Column<string>(type: "text", nullable: true),
                BackChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
                AllowOfflineAccess = table.Column<bool>(type: "boolean", nullable: false),
                IdentityTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "text", nullable: true),
                AccessTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                AuthorizationCodeLifetime = table.Column<int>(type: "integer", nullable: false),
                ConsentLifetime = table.Column<int>(type: "integer", nullable: true),
                AbsoluteRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                SlidingRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                RefreshTokenUsage = table.Column<int>(type: "integer", nullable: false),
                UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "boolean", nullable: false),
                RefreshTokenExpiration = table.Column<int>(type: "integer", nullable: false),
                AccessTokenType = table.Column<int>(type: "integer", nullable: false),
                EnableLocalLogin = table.Column<bool>(type: "boolean", nullable: false),
                IncludeJwtId = table.Column<bool>(type: "boolean", nullable: false),
                AlwaysSendClientClaims = table.Column<bool>(type: "boolean", nullable: false),
                ClientClaimsPrefix = table.Column<string>(type: "text", nullable: true),
                PairWiseSubjectSalt = table.Column<string>(type: "text", nullable: true),
                UserSsoLifetime = table.Column<int>(type: "integer", nullable: true),
                UserCodeType = table.Column<string>(type: "text", nullable: true),
                DeviceCodeLifetime = table.Column<int>(type: "integer", nullable: false),
                CibaLifetime = table.Column<int>(type: "integer", nullable: true),
                PollingInterval = table.Column<int>(type: "integer", nullable: true),
                CoordinateLifetimeWithUserSession = table.Column<bool>(type: "boolean", nullable: true),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                LastAccessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                NonEditable = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DataProtectionKeys",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                FriendlyName = table.Column<string>(type: "text", nullable: true),
                Xml = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DeviceFlowCodes",
            schema: "IdentityServer",
            columns: table => new
            {
                UserCode = table.Column<string>(type: "text", nullable: false),
                DeviceCode = table.Column<string>(type: "text", nullable: true),
                SubjectId = table.Column<string>(type: "text", nullable: true),
                SessionId = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                Data = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeviceFlowCodes", x => x.UserCode);
            });

        migrationBuilder.CreateTable(
            name: "IdentityProviders",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scheme = table.Column<string>(type: "text", nullable: true),
                DisplayName = table.Column<string>(type: "text", nullable: true),
                Enabled = table.Column<bool>(type: "boolean", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true),
                Properties = table.Column<string>(type: "text", nullable: true),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                LastAccessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                NonEditable = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityProviders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "IdentityResources",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>(type: "boolean", nullable: false),
                Name = table.Column<string>(type: "text", nullable: true),
                DisplayName = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                Required = table.Column<bool>(type: "boolean", nullable: false),
                Emphasize = table.Column<bool>(type: "boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                NonEditable = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Keys",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<string>(type: "text", nullable: false),
                Version = table.Column<int>(type: "integer", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Use = table.Column<string>(type: "text", nullable: true),
                Algorithm = table.Column<string>(type: "text", nullable: true),
                IsX509Certificate = table.Column<bool>(type: "boolean", nullable: false),
                DataProtected = table.Column<bool>(type: "boolean", nullable: false),
                Data = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Keys", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PersistedGrants",
            schema: "IdentityServer",
            columns: table => new
            {
                Key = table.Column<string>(type: "text", nullable: false),
                Id = table.Column<long>(type: "bigint", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true),
                SubjectId = table.Column<string>(type: "text", nullable: true),
                SessionId = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                ConsumedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                Data = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PersistedGrants", x => x.Key);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CreatedBy = table.Column<string>(type: "text", nullable: true),
                LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ServerSideSessions",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Key = table.Column<string>(type: "text", nullable: true),
                Scheme = table.Column<string>(type: "text", nullable: true),
                SubjectId = table.Column<string>(type: "text", nullable: true),
                SessionId = table.Column<string>(type: "text", nullable: true),
                DisplayName = table.Column<string>(type: "text", nullable: true),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Renewed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                Data = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<string>(type: "text", nullable: false),
                FirstName = table.Column<string>(type: "text", nullable: true),
                LastName = table.Column<string>(type: "text", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                DisplayName = table.Column<string>(type: "text", nullable: true),
                Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                ProfilePicture = table.Column<string>(type: "text", nullable: true),
                GravatarEmail = table.Column<string>(type: "text", nullable: true),
                Credits = table.Column<double>(type: "double precision", nullable: false),
                DiscordId = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CreatedBy = table.Column<string>(type: "text", nullable: true),
                LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                ObjectId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: true),
                SecurityStamp = table.Column<string>(type: "text", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                PhoneNumber = table.Column<string>(type: "text", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ApiResourceClaims",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalSchema: "IdentityServer",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ApiResourceProperties",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                Key = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalSchema: "IdentityServer",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ApiResourceScopes",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>(type: "text", nullable: true),
                ApiResourceId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalSchema: "IdentityServer",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ApiResourceSecrets",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true),
                Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                Type = table.Column<string>(type: "text", nullable: true),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                    column: x => x.ApiResourceId,
                    principalSchema: "IdentityServer",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ApiScopeClaims",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>(type: "integer", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiScopeClaims_ApiScopes_ScopeId",
                    column: x => x.ScopeId,
                    principalSchema: "IdentityServer",
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ApiScopeProperties",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>(type: "integer", nullable: false),
                Key = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiScopeProperties_ApiScopes_ScopeId",
                    column: x => x.ScopeId,
                    principalSchema: "IdentityServer",
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Products",
            schema: "Catalog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Rate = table.Column<decimal>(type: "numeric", nullable: false),
                ImagePath = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedBy = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                DeletedBy = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey(
                    name: "FK_Products_Brands_BrandId",
                    column: x => x.BrandId,
                    principalSchema: "Catalog",
                    principalTable: "Brands",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientClaims",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Type = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientClaims_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientCorsOrigins",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Origin = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientCorsOrigins_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientGrantTypes",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                GrantType = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientGrantTypes_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientIdPRestrictions",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Provider = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientIdPRestrictions_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientPostLogoutRedirectUris",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostLogoutRedirectUri = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientProperties",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>(type: "integer", nullable: false),
                Key = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientProperties", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientProperties_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientRedirectUris",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RedirectUri = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientRedirectUris_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientScopes",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>(type: "text", nullable: true),
                ClientId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientScopes", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientScopes_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ClientSecrets",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>(type: "integer", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true),
                Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                Type = table.Column<string>(type: "text", nullable: true),
                Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                table.ForeignKey(
                    name: "FK_ClientSecrets_Clients_ClientId",
                    column: x => x.ClientId,
                    principalSchema: "IdentityServer",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "IdentityResourceClaims",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                    column: x => x.IdentityResourceId,
                    principalSchema: "IdentityServer",
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "IdentityResourceProperties",
            schema: "IdentityServer",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
                Key = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                table.ForeignKey(
                    name: "FK_IdentityResourceProperties_IdentityResources_IdentityResour~",
                    column: x => x.IdentityResourceId,
                    principalSchema: "IdentityServer",
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoleClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                CreatedBy = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                RoleId = table.Column<string>(type: "text", nullable: false),
                ClaimType = table.Column<string>(type: "text", nullable: true),
                ClaimValue = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoleClaims_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "Identity",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<string>(type: "text", nullable: false),
                ClaimType = table.Column<string>(type: "text", nullable: true),
                ClaimValue = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserClaims_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserLogins",
            schema: "Identity",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "text", nullable: false),
                ProviderKey = table.Column<string>(type: "text", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                UserId = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_UserLogins_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            schema: "Identity",
            columns: table => new
            {
                UserId = table.Column<string>(type: "text", nullable: false),
                RoleId = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "Identity",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserTokens",
            schema: "Identity",
            columns: table => new
            {
                UserId = table.Column<string>(type: "text", nullable: false),
                LoginProvider = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_UserTokens_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ApiResourceClaims_ApiResourceId",
            schema: "IdentityServer",
            table: "ApiResourceClaims",
            column: "ApiResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_ApiResourceProperties_ApiResourceId",
            schema: "IdentityServer",
            table: "ApiResourceProperties",
            column: "ApiResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_ApiResourceScopes_ApiResourceId",
            schema: "IdentityServer",
            table: "ApiResourceScopes",
            column: "ApiResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_ApiResourceSecrets_ApiResourceId",
            schema: "IdentityServer",
            table: "ApiResourceSecrets",
            column: "ApiResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_ApiScopeClaims_ScopeId",
            schema: "IdentityServer",
            table: "ApiScopeClaims",
            column: "ScopeId");

        migrationBuilder.CreateIndex(
            name: "IX_ApiScopeProperties_ScopeId",
            schema: "IdentityServer",
            table: "ApiScopeProperties",
            column: "ScopeId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientClaims_ClientId",
            schema: "IdentityServer",
            table: "ClientClaims",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientCorsOrigins_ClientId",
            schema: "IdentityServer",
            table: "ClientCorsOrigins",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientGrantTypes_ClientId",
            schema: "IdentityServer",
            table: "ClientGrantTypes",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientIdPRestrictions_ClientId",
            schema: "IdentityServer",
            table: "ClientIdPRestrictions",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientPostLogoutRedirectUris_ClientId",
            schema: "IdentityServer",
            table: "ClientPostLogoutRedirectUris",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientProperties_ClientId",
            schema: "IdentityServer",
            table: "ClientProperties",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientRedirectUris_ClientId",
            schema: "IdentityServer",
            table: "ClientRedirectUris",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientScopes_ClientId",
            schema: "IdentityServer",
            table: "ClientScopes",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ClientSecrets_ClientId",
            schema: "IdentityServer",
            table: "ClientSecrets",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_IdentityResourceClaims_IdentityResourceId",
            schema: "IdentityServer",
            table: "IdentityResourceClaims",
            column: "IdentityResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_IdentityResourceProperties_IdentityResourceId",
            schema: "IdentityServer",
            table: "IdentityResourceProperties",
            column: "IdentityResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_Products_BrandId",
            schema: "Catalog",
            table: "Products",
            column: "BrandId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleClaims_RoleId",
            schema: "Identity",
            table: "RoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            schema: "Identity",
            table: "Roles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserClaims_UserId",
            schema: "Identity",
            table: "UserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserLogins_UserId",
            schema: "Identity",
            table: "UserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId",
            schema: "Identity",
            table: "UserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            schema: "Identity",
            table: "Users",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            schema: "Identity",
            table: "Users",
            column: "NormalizedUserName",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ApiResourceClaims",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ApiResourceProperties",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ApiResourceScopes",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ApiResourceSecrets",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ApiScopeClaims",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ApiScopeProperties",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "AuditTrails",
            schema: "Auditing");

        migrationBuilder.DropTable(
            name: "ClientClaims",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientCorsOrigins",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientGrantTypes",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientIdPRestrictions",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientPostLogoutRedirectUris",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientProperties",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientRedirectUris",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientScopes",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ClientSecrets",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "DataProtectionKeys",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "DeviceFlowCodes",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "IdentityProviders",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "IdentityResourceClaims",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "IdentityResourceProperties",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "Keys",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "PersistedGrants",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "Products",
            schema: "Catalog");

        migrationBuilder.DropTable(
            name: "RoleClaims",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "ServerSideSessions",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "UserClaims",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "UserLogins",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "UserRoles",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "UserTokens",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "ApiResources",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "ApiScopes",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "Clients",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "IdentityResources",
            schema: "IdentityServer");

        migrationBuilder.DropTable(
            name: "Brands",
            schema: "Catalog");

        migrationBuilder.DropTable(
            name: "Roles",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "Identity");
    }
}