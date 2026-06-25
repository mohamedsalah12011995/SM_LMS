#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class UserApplicationExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnswerTotalTime",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CertificateThemeId",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationExam_CertificateThemeId",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                column: "CertificateThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserApplicationExam_CertificateThemes_CertificateThemeId",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                column: "CertificateThemeId",
                principalSchema: "Exams",
                principalTable: "CertificateThemes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApplicationExam_CertificateThemes_CertificateThemeId",
                schema: "ExamStandalone",
                table: "UserApplicationExam");

            migrationBuilder.DropIndex(
                name: "IX_UserApplicationExam_CertificateThemeId",
                schema: "ExamStandalone",
                table: "UserApplicationExam");

            migrationBuilder.DropColumn(
                name: "AnswerTotalTime",
                schema: "ExamStandalone",
                table: "UserApplicationExam");

            migrationBuilder.DropColumn(
                name: "CertificateThemeId",
                schema: "ExamStandalone",
                table: "UserApplicationExam");
        }
    }
}
