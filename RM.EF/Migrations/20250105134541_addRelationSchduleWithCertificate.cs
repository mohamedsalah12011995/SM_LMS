using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addRelationSchduleWithCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CertificateId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_CertificateId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCourseSchedule_Certificates_CertificateId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule",
                column: "CertificateId",
                principalSchema: "Exams",
                principalTable: "Certificates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCourseSchedule_Certificates_CertificateId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCourseSchedule_CertificateId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                schema: "ExamTrainingCourses",
                table: "TrainingCourseSchedule");
        }
    }
}
