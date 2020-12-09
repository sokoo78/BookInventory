using Microsoft.EntityFrameworkCore.Migrations;

namespace BookInventory.Data.Migrations
{
    public partial class ExtendBookModelWithAgeLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nation",
                table: "Author");

            migrationBuilder.AddColumn<int>(
                name: "AgeLimit",
                table: "Book",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Author",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeLimit",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Author");

            migrationBuilder.AddColumn<string>(
                name: "Nation",
                table: "Author",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
