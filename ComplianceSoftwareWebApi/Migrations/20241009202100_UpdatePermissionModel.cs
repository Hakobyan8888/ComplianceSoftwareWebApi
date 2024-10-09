using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplianceSoftwareWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Permissions",
                newName: "PermissionType");

            migrationBuilder.AddColumn<string>(
                name: "PermissionName",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionName",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "PermissionType",
                table: "Permissions",
                newName: "Name");
        }
    }
}
