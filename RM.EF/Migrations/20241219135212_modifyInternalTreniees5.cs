using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyInternalTreniees5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByNavigationId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_CreatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_UpdatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseTraniees_CreatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseTraniees_DeletedByNavigationId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "DeletedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseTraniees_UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCourseTraniees_Users_CreatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCourseTraniees_Users_DeletedByNavigationId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "DeletedByNavigationId",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCourseTraniees_Users_UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseTrainees_Users_CreatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseTrainees_Users_UpdatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCourseTraniees_Users_CreatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCourseTraniees_Users_DeletedByNavigationId",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCourseTraniees_Users_UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseTrainees_Users_CreatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseTrainees_Users_UpdatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseTrainees_CreatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseTrainees_UpdatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCourseTraniees_CreatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCourseTraniees_DeletedByNavigationId",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCourseTraniees_UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "DeletedByNavigationId",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "Exams",
                table: "ExternalCourseTraniees");
        }
    }
}
