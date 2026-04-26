using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyInternalTreniees2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_CourseId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseTrainees_TrainingCourses_CourseId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "CourseId",
                principalSchema: "Exams",
                principalTable: "TrainingCourses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseTrainees_TrainingCourses_CourseId",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseTrainees_CourseId",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "Exams",
                table: "InternalCourseTrainees");

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
    }
}
