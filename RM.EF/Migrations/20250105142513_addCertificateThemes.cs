using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addCertificateThemes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CertificateThemeId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CertificateThemes",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateThemes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_CertificateThemeId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule",
                column: "CertificateThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCourseSchedule_CertificateThemes_CertificateThemeId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule",
                column: "CertificateThemeId",
                principalSchema: "Exams",
                principalTable: "CertificateThemes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCourseSchedule_CertificateThemes_CertificateThemeId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule");

            migrationBuilder.DropTable(
                name: "CertificateThemes",
                schema: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCourseSchedule_CertificateThemeId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule");

            migrationBuilder.DropColumn(
                name: "CertificateThemeId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule");
        }
    }
}
