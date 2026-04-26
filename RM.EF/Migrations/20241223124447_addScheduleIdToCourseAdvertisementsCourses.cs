using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addScheduleIdToCourseAdvertisementsCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisementsCourses_TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "TrainingCourseScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAdvertisementsCourses_TrainingCourseSchedule_TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "TrainingCourseScheduleId",
                principalSchema: "Exams",
                principalTable: "TrainingCourseSchedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAdvertisementsCourses_TrainingCourseSchedule_TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses");

            migrationBuilder.DropIndex(
                name: "IX_CourseAdvertisementsCourses_TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses");

            migrationBuilder.DropColumn(
                name: "TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses");
        }
    }
}
