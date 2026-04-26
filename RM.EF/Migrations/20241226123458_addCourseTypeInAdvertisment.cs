using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addCourseTypeInAdvertisment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingCourseTypeId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementsCourses_TrainingCourseTypeId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "TrainingCourseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementsCourses_TrainingCourseTypes_TrainingCourseTypeId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "TrainingCourseTypeId",
                principalSchema: "Exams",
                principalTable: "TrainingCourseTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementsCourses_TrainingCourseTypes_TrainingCourseTypeId",
                schema: "Exams",
                table: "AdvertisementsCourses");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisementsCourses_TrainingCourseTypeId",
                schema: "Exams",
                table: "AdvertisementsCourses");

            migrationBuilder.DropColumn(
                name: "TrainingCourseTypeId",
                schema: "Exams",
                table: "AdvertisementsCourses");
        }
    }
}
