using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "Survey",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThemeId",
                schema: "Survey",
                table: "Surveys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "Survey",
                table: "QuestionType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "Survey",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFiltration",
                schema: "Survey",
                table: "Questions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "Survey",
                table: "DataSource",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Themes",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_ThemeId",
                schema: "Survey",
                table: "Surveys",
                column: "ThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Themes_ThemeId",
                schema: "Survey",
                table: "Surveys",
                column: "ThemeId",
                principalSchema: "Survey",
                principalTable: "Themes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Themes_ThemeId",
                schema: "Survey",
                table: "Surveys");

            migrationBuilder.DropTable(
                name: "Themes",
                schema: "Survey");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_ThemeId",
                schema: "Survey",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "Survey",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "ThemeId",
                schema: "Survey",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "Survey",
                table: "QuestionType");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "Survey",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsFiltration",
                schema: "Survey",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "Survey",
                table: "DataSource");
        }
    }
}
