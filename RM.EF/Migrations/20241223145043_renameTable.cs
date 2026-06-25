#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class renameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAdvertisementsCourses",
                schema: "Exams");

            migrationBuilder.CreateTable(
                name: "AdvertisementsCourses",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAdvertisementId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    TrainingCourseScheduleId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementsCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementsCourses_CourseAdvertisements_CourseAdvertisementId",
                        column: x => x.CourseAdvertisementId,
                        principalSchema: "Exams",
                        principalTable: "CourseAdvertisements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementsCourses_TrainingCourseSchedule_TrainingCourseScheduleId",
                        column: x => x.TrainingCourseScheduleId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementsCourses_TrainingCourses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementsCourses_CourseAdvertisementId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "CourseAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementsCourses_CourseId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementsCourses_TrainingCourseScheduleId",
                schema: "Exams",
                table: "AdvertisementsCourses",
                column: "TrainingCourseScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementsCourses",
                schema: "Exams");

            migrationBuilder.CreateTable(
                name: "CourseAdvertisementsCourses",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAdvertisementId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    TrainingCourseScheduleId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAdvertisementsCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAdvertisementsCourses_CourseAdvertisements_CourseAdvertisementId",
                        column: x => x.CourseAdvertisementId,
                        principalSchema: "Exams",
                        principalTable: "CourseAdvertisements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseAdvertisementsCourses_TrainingCourseSchedule_TrainingCourseScheduleId",
                        column: x => x.TrainingCourseScheduleId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseAdvertisementsCourses_TrainingCourses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisementsCourses_CourseAdvertisementId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "CourseAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisementsCourses_CourseId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisementsCourses_TrainingCourseScheduleId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "TrainingCourseScheduleId");
        }
    }
}
