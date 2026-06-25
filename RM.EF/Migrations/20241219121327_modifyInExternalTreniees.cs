#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyInExternalTreniees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnWaitingList",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseTraniees_CourseId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCourseTraniees_TrainingCourses_CourseId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "CourseId",
                principalSchema: "Exams",
                principalTable: "TrainingCourses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCourseTraniees_TrainingCourses_CourseId",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCourseTraniees_CourseId",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.AddColumn<bool>(
                name: "OnWaitingList",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "bit",
                nullable: true);
        }
    }
}
