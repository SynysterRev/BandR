using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BandR.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceConversationArchivingWithIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "conversations",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.Sql("""
                UPDATE conversations
                SET "IsActive" = ("Status" = 0);
                """);

            migrationBuilder.DropColumn(
                name: "ArchiveReason",
                table: "conversations");

            migrationBuilder.DropColumn(
                name: "ArchivedAt",
                table: "conversations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "conversations");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArchiveReason",
                table: "conversations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedAt",
                table: "conversations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "conversations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE conversations
                SET "Status" = CASE WHEN "IsActive" THEN 0 ELSE 1 END;
                """);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "conversations");
        }
    }
}
