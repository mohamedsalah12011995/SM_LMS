using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class removeCoulmn_IsAttendedFromInternalExamTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAttendExam",
                schema: "Exams",
                table: "InternalCourseExams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAttendExam",
                schema: "Exams",
                table: "InternalCourseExams",
                type: "bit",
                nullable: true);
        }
    }
}
