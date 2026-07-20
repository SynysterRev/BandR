using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BandR.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationArchiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
