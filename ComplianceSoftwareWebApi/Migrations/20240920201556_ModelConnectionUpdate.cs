using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplianceSoftwareWebApi.Migrations
{
    /// <inheritdoc />
    public partial class ModelConnectionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "Permissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Companies",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions",
                columns: new[] { "UserId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPermissions");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Permissions",
                newName: "PermissionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Companies",
                newName: "CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId" });
        }
    }
}
