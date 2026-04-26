using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class examActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamExternalTranieesAnswerAction",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalCourseExamsId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamExternalTranieesAnswerAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamExternalTranieesAnswerAction_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamExternalTranieesAnswerAction_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExamInternalTranieesAnswerAction",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InternalCourseExamsId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamInternalTranieesAnswerAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamInternalTranieesAnswerAction_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamInternalTranieesAnswerAction_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExamExternalTranieesQuestionAnswer",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    DataSourceId = table.Column<int>(type: "int", nullable: true),
                    ExamExternalTranieesAnswerActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamExternalTranieesQuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamExternalTranieesQuestionAnswer_DataSource_DataSourceId",
                        column: x => x.DataSourceId,
                        principalSchema: "Exams",
                        principalTable: "DataSource",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamExternalTranieesQuestionAnswer_ExamExternalTranieesAnswerAction_ExamExternalTranieesAnswerActionId",
                        column: x => x.ExamExternalTranieesAnswerActionId,
                        principalSchema: "Exams",
                        principalTable: "ExamExternalTranieesAnswerAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamExternalTranieesQuestionAnswer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exams",
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamInternalTranieesQuestionAnswer",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    DataSourceId = table.Column<int>(type: "int", nullable: true),
                    ExamInternalTranieesAnswerActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamInternalTranieesQuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamInternalTranieesQuestionAnswer_DataSource_DataSourceId",
                        column: x => x.DataSourceId,
                        principalSchema: "Exams",
                        principalTable: "DataSource",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamInternalTranieesQuestionAnswer_ExamInternalTranieesAnswerAction_ExamInternalTranieesAnswerActionId",
                        column: x => x.ExamInternalTranieesAnswerActionId,
                        principalSchema: "Exams",
                        principalTable: "ExamInternalTranieesAnswerAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamInternalTranieesQuestionAnswer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exams",
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamExternalTranieesAnswerAction_CreatedBy",
                schema: "Exams",
                table: "ExamExternalTranieesAnswerAction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExamExternalTranieesAnswerAction_ExamId",
                schema: "Exams",
                table: "ExamExternalTranieesAnswerAction",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamExternalTranieesQuestionAnswer_DataSourceId",
                schema: "Exams",
                table: "ExamExternalTranieesQuestionAnswer",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamExternalTranieesQuestionAnswer_ExamExternalTranieesAnswerActionId",
                schema: "Exams",
                table: "ExamExternalTranieesQuestionAnswer",
                column: "ExamExternalTranieesAnswerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamExternalTranieesQuestionAnswer_QuestionId",
                schema: "Exams",
                table: "ExamExternalTranieesQuestionAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamInternalTranieesAnswerAction_CreatedBy",
                schema: "Exams",
                table: "ExamInternalTranieesAnswerAction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExamInternalTranieesAnswerAction_ExamId",
                schema: "Exams",
                table: "ExamInternalTranieesAnswerAction",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamInternalTranieesQuestionAnswer_DataSourceId",
                schema: "Exams",
                table: "ExamInternalTranieesQuestionAnswer",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamInternalTranieesQuestionAnswer_ExamInternalTranieesAnswerActionId",
                schema: "Exams",
                table: "ExamInternalTranieesQuestionAnswer",
                column: "ExamInternalTranieesAnswerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamInternalTranieesQuestionAnswer_QuestionId",
                schema: "Exams",
                table: "ExamInternalTranieesQuestionAnswer",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamExternalTranieesQuestionAnswer",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "ExamInternalTranieesQuestionAnswer",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "ExamExternalTranieesAnswerAction",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "ExamInternalTranieesAnswerAction",
                schema: "Exams");
        }
    }
}
