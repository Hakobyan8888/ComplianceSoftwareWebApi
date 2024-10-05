using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplianceSoftwareWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Companies",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "Industry",
                table: "Companies",
                newName: "StreetAddress");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Companies",
                newName: "StateOfFormation");

            migrationBuilder.AddColumn<int>(
                name: "BusinessIndustryCode",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "IndustryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndustryTypeCode = table.Column<int>(type: "int", nullable: false),
                    IndustryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryTypes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "BusinessIndustryCode",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Companies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "Companies",
                newName: "Industry");

            migrationBuilder.RenameColumn(
                name: "StateOfFormation",
                table: "Companies",
                newName: "Address");
        }
    }
}
