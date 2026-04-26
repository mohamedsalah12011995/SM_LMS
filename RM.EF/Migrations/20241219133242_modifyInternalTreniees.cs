using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyInternalTreniees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                schema: "Exams",
                table: "InternalCourseExams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_CourseId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseExams_TrainingCourses_CourseId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "CourseId",
                principalSchema: "Exams",
                principalTable: "TrainingCourses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseExams_TrainingCourses_CourseId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseExams_CourseId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "Exams",
                table: "InternalCourseExams");
        }
    }
}
