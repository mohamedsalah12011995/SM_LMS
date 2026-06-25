#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addCertificateLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificateLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateId = table.Column<int>(type: "int", nullable: true),
                    UserApplicationExamId = table.Column<int>(type: "int", nullable: true),
                    ExternalCourseExamsId = table.Column<int>(type: "int", nullable: true),
                    InternalCourseExamsId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateLog_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalSchema: "Exams",
                        principalTable: "Certificates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CertificateLog_ExternalCourseExams_ExternalCourseExamsId",
                        column: x => x.ExternalCourseExamsId,
                        principalSchema: "ExamTraining",
                        principalTable: "ExternalCourseExams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CertificateLog_InternalCourseExams_InternalCourseExamsId",
                        column: x => x.InternalCourseExamsId,
                        principalSchema: "ExamTraining",
                        principalTable: "InternalCourseExams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CertificateLog_UserApplicationExam_UserApplicationExamId",
                        column: x => x.UserApplicationExamId,
                        principalSchema: "ExamStandalone",
                        principalTable: "UserApplicationExam",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateLog_CertificateId",
                table: "CertificateLog",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateLog_ExternalCourseExamsId",
                table: "CertificateLog",
                column: "ExternalCourseExamsId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateLog_InternalCourseExamsId",
                table: "CertificateLog",
                column: "InternalCourseExamsId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateLog_UserApplicationExamId",
                table: "CertificateLog",
                column: "UserApplicationExamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateLog");
        }
    }
}
