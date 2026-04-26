using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addExamIdToCourseExamTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAttendedExam",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                schema: "Exams",
                table: "InternalCourseExams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_ExamId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseExams_Exams_ExamId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "ExamId",
                principalSchema: "Exams",
                principalTable: "Exams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseExams_Exams_ExamId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseExams_ExamId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "IsAttendedExam",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "ExamId",
                schema: "Exams",
                table: "InternalCourseExams");
        }
    }
}
