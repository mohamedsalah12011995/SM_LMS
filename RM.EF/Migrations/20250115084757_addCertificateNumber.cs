using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addCertificateNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateNumber",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateNumber",
                schema: "ExamTraining",
                table: "InternalCourseExams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateNumber",
                schema: "ExamTraining",
                table: "ExternalCourseExams",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateNumber",
                schema: "ExamStandalone",
                table: "UserApplicationExam");

            migrationBuilder.DropColumn(
                name: "CertificateNumber",
                schema: "ExamTraining",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "CertificateNumber",
                schema: "ExamTraining",
                table: "ExternalCourseExams");
        }
    }
}
