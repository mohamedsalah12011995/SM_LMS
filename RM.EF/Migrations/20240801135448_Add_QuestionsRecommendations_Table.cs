using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class Add_QuestionsRecommendations_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rate",
                schema: "Survey",
                table: "DataSource",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionsRecommendations",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessAverageAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessAverageEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboveAverageAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboveAverageEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsRecommendations_Questions",
                        column: x => x.QuestionId,
                        principalSchema: "Survey",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsRecommendations_QuestionId",
                schema: "Survey",
                table: "QuestionsRecommendations",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsRecommendations",
                schema: "Survey");

            migrationBuilder.DropColumn(
                name: "Rate",
                schema: "Survey",
                table: "DataSource");
        }
    }
}
