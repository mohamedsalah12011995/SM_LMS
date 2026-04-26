using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyInternalTreniees6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalCourseTrainees_References_ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropIndex(
                name: "IX_InternalCourseTrainees_ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalCourseTrainees_References_ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");
        }
    }
}
