using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    public partial class v5_ThemAlias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "tbl_ProductCategories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "tbl_Product",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "tbl_Posts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "tbl_News",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "tbl_Categories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "tbl_Advertisement",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "tbl_ProductCategories");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "tbl_Product");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "tbl_Posts");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "tbl_News");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "tbl_Categories");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "tbl_Advertisement");
        }
    }
}
