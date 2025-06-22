using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Netflex.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V0_0_2_AddReportEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "keywords",
                newName: "keyword_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "genres",
                newName: "genre_id");

            migrationBuilder.CreateTable(
                name: "reports",
                schema: "dbo",
                columns: table => new
                {
                    report_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    process = table.Column<string>(type: "text", nullable: false, defaultValue: "Open"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reports", x => x.report_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reports",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "keyword_id",
                schema: "dbo",
                table: "keywords",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                schema: "dbo",
                table: "genres",
                newName: "id");
        }
    }
}
