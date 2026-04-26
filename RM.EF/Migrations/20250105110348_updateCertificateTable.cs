using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class updateCertificateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssueDate",
                schema: "Exams",
                table: "Certificates",
                newName: "UpdatedDate");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Exams",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "Exams",
                table: "Certificates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Exams",
                table: "Certificates",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "Exams",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CreatedBy",
                schema: "Exams",
                table: "Certificates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_UpdatedBy",
                schema: "Exams",
                table: "Certificates",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Users_CreatedBy",
                schema: "Exams",
                table: "Certificates",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Users_UpdatedBy",
                schema: "Exams",
                table: "Certificates",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Users_CreatedBy",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Users_UpdatedBy",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_CreatedBy",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_UpdatedBy",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Exams",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "Exams",
                table: "Certificates",
                newName: "IssueDate");
        }
    }
}
