using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class updateExamTraineeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCourseExams_Users_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseExams_Users_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseExams_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCourseExams_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseExams_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams",
                column: "DeletedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCourseExams_Users_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "ExternalCourseExams",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseExams_Users_DeletedBy",
                schema: "ExamTrainingCourses",
                table: "InternalCourseExams",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
