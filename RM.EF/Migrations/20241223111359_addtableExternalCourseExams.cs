#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addtableExternalCourseExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalCourseExams",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingCourseScheduleId = table.Column<int>(type: "int", nullable: true),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    ExternalCourseTranieeId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnswerTotalTime = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<double>(type: "float", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalCourseExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalCourseExams_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExternalCourseExams_ExternalCourseTraniees_ExternalCourseTranieeId",
                        column: x => x.ExternalCourseTranieeId,
                        principalSchema: "Exams",
                        principalTable: "ExternalCourseTraniees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExternalCourseExams_TrainingCourseSchedule_TrainingCourseScheduleId",
                        column: x => x.TrainingCourseScheduleId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExternalCourseExams_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExternalCourseExams_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExternalCourseExams_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_CreatedBy",
                schema: "Exams",
                table: "ExternalCourseExams",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_DeletedBy",
                schema: "Exams",
                table: "ExternalCourseExams",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_ExamId",
                schema: "Exams",
                table: "ExternalCourseExams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_ExternalCourseTranieeId",
                schema: "Exams",
                table: "ExternalCourseExams",
                column: "ExternalCourseTranieeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_TrainingCourseScheduleId",
                schema: "Exams",
                table: "ExternalCourseExams",
                column: "TrainingCourseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseExams",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalCourseExams",
                schema: "Exams");
        }
    }
}
