using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BandR.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameByUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "musicians");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "musicians",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "musicians",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_locations_City",
                table: "locations",
                column: "City",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_locations_City",
                table: "locations");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "musicians",
                newName: "LastName");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "musicians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "musicians",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
