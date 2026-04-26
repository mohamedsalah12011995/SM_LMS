using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class deleteCourseIdFromAdvCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementsCourses_TrainingCourses_CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisementsCourses_CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementsCourses_CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementsCourses_TrainingCourses_CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "CourseId",
                principalSchema: "Exams",
                principalTable: "TrainingCourses",
                principalColumn: "Id");
        }
    }
}
