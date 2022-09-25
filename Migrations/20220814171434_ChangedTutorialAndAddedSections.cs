using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyTest.Migrations
{
    public partial class ChangedTutorialAndAddedSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Tutorials_TutorialId",
                table: "Topics");

            migrationBuilder.RenameColumn(
                name: "TutorialId",
                table: "Topics",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_TutorialId",
                table: "Topics",
                newName: "IX_Topics_SectionId");

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TutorialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Tutorials_TutorialId",
                        column: x => x.TutorialId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Section_TutorialId",
                table: "Section",
                column: "TutorialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Section_SectionId",
                table: "Topics",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Section_SectionId",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Topics",
                newName: "TutorialId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_SectionId",
                table: "Topics",
                newName: "IX_Topics_TutorialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Tutorials_TutorialId",
                table: "Topics",
                column: "TutorialId",
                principalTable: "Tutorials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
