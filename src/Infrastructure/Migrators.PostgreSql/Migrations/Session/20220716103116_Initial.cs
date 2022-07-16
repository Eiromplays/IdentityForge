using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Migrators.PostgreSql.Migrations.Session
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Renewed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Ticket = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_ApplicationName_Key",
                table: "UserSessions",
                columns: new[] { "ApplicationName", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_ApplicationName_SessionId",
                table: "UserSessions",
                columns: new[] { "ApplicationName", "SessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_ApplicationName_SubjectId_SessionId",
                table: "UserSessions",
                columns: new[] { "ApplicationName", "SubjectId", "SessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_Expires",
                table: "UserSessions",
                column: "Expires");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSessions");
        }
    }
}
