using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addReferencetoCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                schema: "Exams",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ReferenceId",
                schema: "Exams",
                table: "Certificates",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_References_ReferenceId",
                schema: "Exams",
                table: "Certificates",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_References_ReferenceId",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_ReferenceId",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "Exams",
                table: "Certificates");
        }
    }
}
