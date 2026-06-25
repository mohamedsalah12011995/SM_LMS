#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class SurveyRecommendations_Table_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Surveys");

            //migrationBuilder.AddColumn<int>(
            //    name: "Rate",
            //    schema: "Survey",
            //    table: "DataSource",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.DropTable(
                name: "QuestionsRecommendations",
                schema: "Survey");

            migrationBuilder.CreateTable(
                name: "SurveyRecommendations",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyRecommendations_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SurveyRecommendations_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SurveyRecommendations_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SurveySettings",
                schema: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CronTypeId = table.Column<int>(type: "int", nullable: true),
                    Emails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Survey_SurveySettings",
                        column: x => x.SurveyId,
                        principalSchema: "Survey",
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsRecommendations",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessAverageId = table.Column<int>(type: "int", nullable: true),
                    AverageId = table.Column<int>(type: "int", nullable: true),
                    AboveAverageId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRecommendationAboveAverages",
                        column: x => x.AboveAverageId,
                        principalSchema: "Survey",
                        principalTable: "SurveyRecommendations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionRecommendationAverages",
                        column: x => x.AverageId,
                        principalSchema: "Survey",
                        principalTable: "SurveyRecommendations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionRecommendationLessAverages",
                        column: x => x.LessAverageId,
                        principalSchema: "Survey",
                        principalTable: "SurveyRecommendations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionsRecommendations_Questions",
                        column: x => x.QuestionId,
                        principalSchema: "Survey",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsRecommendations_AboveAverageId",
                schema: "Survey",
                table: "QuestionsRecommendations",
                column: "AboveAverageId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsRecommendations_AverageId",
                schema: "Survey",
                table: "QuestionsRecommendations",
                column: "AverageId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsRecommendations_LessAverageId",
                schema: "Survey",
                table: "QuestionsRecommendations",
                column: "LessAverageId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsRecommendations_QuestionId",
                schema: "Survey",
                table: "QuestionsRecommendations",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyRecommendations_CreatedBy",
                schema: "Survey",
                table: "SurveyRecommendations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyRecommendations_ReferenceId",
                schema: "Survey",
                table: "SurveyRecommendations",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyRecommendations_UpdatedBy",
                schema: "Survey",
                table: "SurveyRecommendations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveySettings_SurveyId",
                schema: "Surveys",
                table: "SurveySettings",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsRecommendations",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "SurveySettings",
                schema: "Surveys");

            migrationBuilder.DropTable(
                name: "SurveyRecommendations",
                schema: "Survey");

            //migrationBuilder.DropColumn(
            //    name: "Rate",
            //    schema: "Survey",
            //    table: "DataSource");
        }
    }
}
