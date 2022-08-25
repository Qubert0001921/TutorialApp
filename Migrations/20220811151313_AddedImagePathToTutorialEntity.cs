using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyTest.Migrations
{
    public partial class AddedImagePathToTutorialEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Tutorials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Tutorials");
        }
    }
}
