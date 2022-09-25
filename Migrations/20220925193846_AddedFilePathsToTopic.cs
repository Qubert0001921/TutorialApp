using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyTest.Migrations
{
    public partial class AddedFilePathsToTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentFilePath",
                table: "Topics",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideFilePath",
                table: "Topics",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentFilePath",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "VideFilePath",
                table: "Topics");
        }
    }
}
