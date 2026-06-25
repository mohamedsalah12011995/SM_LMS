#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class updateExternalAndInternalTrainees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificationNumber",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.AddColumn<string>(
                name: "CertificationNumber",
                schema: "ExamTrainingCourses",
                table: "InternalCourseTrainees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSentCertification",
                schema: "ExamTrainingCourses",
                table: "InternalCourseTrainees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificationNumber",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseTraniees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSentCertification",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseTraniees",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificationNumber",
                schema: "ExamTrainingCourses",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "IsSentCertification",
                schema: "ExamTrainingCourses",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "CertificationNumber",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "IsSentCertification",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseTraniees");

            migrationBuilder.AddColumn<string>(
                name: "CertificationNumber",
                schema: "Exams",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
