using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyTest.Migrations
{
    public partial class AddedPriceAndIsPublicToTutorialEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Tutorials",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Tutorials",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Tutorials");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tutorials");
        }
    }
}
