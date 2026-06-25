#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifyInExternalTreniees_Grade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GradeType",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseTraniees_GradeType",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "GradeType");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCourseTraniees_MajorLookups_GradeType",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "GradeType",
                principalSchema: "dbo",
                principalTable: "MajorLookups",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCourseTraniees_MajorLookups_GradeType",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCourseTraniees_GradeType",
                schema: "Exams",
                table: "ExternalCourseTraniees");

            migrationBuilder.AlterColumn<int>(
                name: "GradeType",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
