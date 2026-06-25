#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class updateCertificateTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                schema: "Exams",
                table: "CertificateThemes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                schema: "Exams",
                table: "CertificateThemes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleAr",
                schema: "Exams",
                table: "CertificateThemes");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                schema: "Exams",
                table: "CertificateThemes");
        }
    }
}
