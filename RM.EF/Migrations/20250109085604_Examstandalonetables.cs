#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class Examstandalonetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CertificateId",
                schema: "Exams",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CertificateId",
                schema: "Exams",
                table: "Exams",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Certificates_CertificateId",
                schema: "Exams",
                table: "Exams",
                column: "CertificateId",
                principalSchema: "Exams",
                principalTable: "Certificates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Certificates_CertificateId",
                schema: "Exams",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_CertificateId",
                schema: "Exams",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                schema: "Exams",
                table: "Exams");
        }
    }
}
