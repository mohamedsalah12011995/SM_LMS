#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addCertificateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "TrainingCourseTypes",
                schema: "Exams",
                newName: "TrainingCourseTypes",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "TrainingCourseSchedule",
                schema: "Exams",
                newName: "TrainingCourseSchedule",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "TrainingCourses",
                schema: "Exams",
                newName: "TrainingCourses",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "InternalCourseTrainees",
                schema: "Exams",
                newName: "InternalCourseTrainees",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "InternalCourseExams",
                schema: "Exams",
                newName: "InternalCourseExams",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExternalCourseTraniees",
                schema: "Exams",
                newName: "ExternalCourseTraniees",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExternalCourseExams",
                schema: "Exams",
                newName: "ExternalCourseExams",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesQuestionAnswer",
                schema: "Exams",
                newName: "ExamInternalTranieesQuestionAnswer",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesAnswerAction",
                schema: "Exams",
                newName: "ExamInternalTranieesAnswerAction",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesQuestionAnswer",
                schema: "Exams",
                newName: "ExamExternalTranieesQuestionAnswer",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesAnswerAction",
                schema: "Exams",
                newName: "ExamExternalTranieesAnswerAction",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "CourseAdvertisements",
                schema: "Exams",
                newName: "CourseAdvertisements",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "AdvertisementsCourses",
                schema: "Exams",
                newName: "AdvertisementsCourses",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.CreateTable(
                name: "Certificates",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates",
                schema: "Exams");

            migrationBuilder.RenameTable(
                name: "TrainingCourseTypes",
                schema: "ExamTrainingCourses",
                newName: "TrainingCourseTypes",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "TrainingCourseSchedule",
                schema: "ExamTrainingCourses",
                newName: "TrainingCourseSchedule",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "TrainingCourses",
                schema: "ExamTrainingCourses",
                newName: "TrainingCourses",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "InternalCourseTrainees",
                schema: "ExamTrainingCourses",
                newName: "InternalCourseTrainees",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "InternalCourseExams",
                schema: "ExamTrainingCourses",
                newName: "InternalCourseExams",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "ExternalCourseTraniees",
                schema: "ExamTrainingCourses",
                newName: "ExternalCourseTraniees",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "ExternalCourseExams",
                schema: "ExamTrainingCourses",
                newName: "ExternalCourseExams",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesQuestionAnswer",
                schema: "ExamTrainingCourses",
                newName: "ExamInternalTranieesQuestionAnswer",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesAnswerAction",
                schema: "ExamTrainingCourses",
                newName: "ExamInternalTranieesAnswerAction",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesQuestionAnswer",
                schema: "ExamTrainingCourses",
                newName: "ExamExternalTranieesQuestionAnswer",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesAnswerAction",
                schema: "ExamTrainingCourses",
                newName: "ExamExternalTranieesAnswerAction",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "CourseAdvertisements",
                schema: "ExamTrainingCourses",
                newName: "CourseAdvertisements",
                newSchema: "Exams");

            migrationBuilder.RenameTable(
                name: "AdvertisementsCourses",
                schema: "ExamTrainingCourses",
                newName: "AdvertisementsCourses",
                newSchema: "Exams");
        }
    }
}
