using Microsoft.EntityFrameworkCore.Migrations;

namespace BookInventory.Data.Migrations
{
    public partial class ExtendModelsWithActiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Book",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Author",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Author");
        }
    }
}
