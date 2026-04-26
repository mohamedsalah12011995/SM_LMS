using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyRelationInInternalExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InternalCourseTraineeId",
                schema: "Exams",
                table: "InternalCourseExams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttendedExam",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_InternalCourseTraineeId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "InternalCourseTraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseExams_InternalCourseTrainees_InternalCourseTraineeId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "InternalCourseTraineeId",
                principalSchema: "Exams",
                principalTable: "InternalCourseTrainees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseExams_InternalCourseTrainees_InternalCourseTraineeId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseExams_InternalCourseTraineeId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "InternalCourseTraineeId",
                schema: "Exams",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "IsAttendedExam",
                schema: "Exams",
                table: "ExternalCourseTraniees");
        }
    }
}
