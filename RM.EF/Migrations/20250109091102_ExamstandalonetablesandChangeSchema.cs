using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class ExamstandalonetablesandChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ExamTraining");

            migrationBuilder.EnsureSchema(
                name: "ExamStandalone");

            migrationBuilder.RenameTable(
                name: "TrainingCourseTypes",
                schema: "ExamTrainingCourses",
                newName: "TrainingCourseTypes",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "TrainingCourseSchedule",
                schema: "ExamTrainingCourses",
                newName: "TrainingCourseSchedule",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "TrainingCourses",
                schema: "ExamTrainingCourses",
                newName: "TrainingCourses",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "InternalCourseTrainees",
                schema: "ExamTrainingCourses",
                newName: "InternalCourseTrainees",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "InternalCourseExams",
                schema: "ExamTrainingCourses",
                newName: "InternalCourseExams",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "ExternalCourseTraniees",
                schema: "ExamTrainingCourses",
                newName: "ExternalCourseTraniees",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "ExternalCourseExams",
                schema: "ExamTrainingCourses",
                newName: "ExternalCourseExams",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesQuestionAnswer",
                schema: "ExamTrainingCourses",
                newName: "ExamInternalTranieesQuestionAnswer",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesAnswerAction",
                schema: "ExamTrainingCourses",
                newName: "ExamInternalTranieesAnswerAction",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesQuestionAnswer",
                schema: "ExamTrainingCourses",
                newName: "ExamExternalTranieesQuestionAnswer",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesAnswerAction",
                schema: "ExamTrainingCourses",
                newName: "ExamExternalTranieesAnswerAction",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "CourseAdvertisements",
                schema: "ExamTrainingCourses",
                newName: "CourseAdvertisements",
                newSchema: "ExamTraining");

            migrationBuilder.RenameTable(
                name: "AdvertisementsCourses",
                schema: "ExamTrainingCourses",
                newName: "AdvertisementsCourses",
                newSchema: "ExamTraining");

            migrationBuilder.CreateTable(
                name: "UserApplicationExam",
                schema: "ExamStandalone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<double>(type: "float", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplicationExam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserApplicationExam_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserApplicationExam_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserApplicationExam_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserApplicationExam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamUserApplicationAnswerAction",
                schema: "ExamStandalone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserApplicationExamId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamUserApplicationAnswerAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamUserApplicationAnswerAction_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamUserApplicationAnswerAction_UserApplicationExam_UserApplicationExamId",
                        column: x => x.UserApplicationExamId,
                        principalSchema: "ExamStandalone",
                        principalTable: "UserApplicationExam",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamUserApplicationAnswerAction_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExamUserApplicationQuestionAnswer",
                schema: "ExamStandalone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    DataSourceId = table.Column<int>(type: "int", nullable: true),
                    ExamUserApplicationAnswerActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamUserApplicationQuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamUserApplicationQuestionAnswer_DataSource_DataSourceId",
                        column: x => x.DataSourceId,
                        principalSchema: "Exams",
                        principalTable: "DataSource",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamUserApplicationQuestionAnswer_ExamUserApplicationAnswerAction_ExamUserApplicationAnswerActionId",
                        column: x => x.ExamUserApplicationAnswerActionId,
                        principalSchema: "ExamStandalone",
                        principalTable: "ExamUserApplicationAnswerAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamUserApplicationQuestionAnswer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exams",
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamUserApplicationAnswerAction_CreatedBy",
                schema: "ExamStandalone",
                table: "ExamUserApplicationAnswerAction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUserApplicationAnswerAction_ExamId",
                schema: "ExamStandalone",
                table: "ExamUserApplicationAnswerAction",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUserApplicationAnswerAction_UserApplicationExamId",
                schema: "ExamStandalone",
                table: "ExamUserApplicationAnswerAction",
                column: "UserApplicationExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUserApplicationQuestionAnswer_DataSourceId",
                schema: "ExamStandalone",
                table: "ExamUserApplicationQuestionAnswer",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUserApplicationQuestionAnswer_ExamUserApplicationAnswerActionId",
                schema: "ExamStandalone",
                table: "ExamUserApplicationQuestionAnswer",
                column: "ExamUserApplicationAnswerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUserApplicationQuestionAnswer_QuestionId",
                schema: "ExamStandalone",
                table: "ExamUserApplicationQuestionAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationExam_CreatedBy",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationExam_ExamId",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationExam_UpdatedBy",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationExam_UserId",
                schema: "ExamStandalone",
                table: "UserApplicationExam",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamUserApplicationQuestionAnswer",
                schema: "ExamStandalone");

            migrationBuilder.DropTable(
                name: "ExamUserApplicationAnswerAction",
                schema: "ExamStandalone");

            migrationBuilder.DropTable(
                name: "UserApplicationExam",
                schema: "ExamStandalone");

            migrationBuilder.EnsureSchema(
                name: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "TrainingCourseTypes",
                schema: "ExamTraining",
                newName: "TrainingCourseTypes",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "TrainingCourseSchedule",
                schema: "ExamTraining",
                newName: "TrainingCourseSchedule",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "TrainingCourses",
                schema: "ExamTraining",
                newName: "TrainingCourses",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "InternalCourseTrainees",
                schema: "ExamTraining",
                newName: "InternalCourseTrainees",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "InternalCourseExams",
                schema: "ExamTraining",
                newName: "InternalCourseExams",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExternalCourseTraniees",
                schema: "ExamTraining",
                newName: "ExternalCourseTraniees",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExternalCourseExams",
                schema: "ExamTraining",
                newName: "ExternalCourseExams",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesQuestionAnswer",
                schema: "ExamTraining",
                newName: "ExamInternalTranieesQuestionAnswer",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamInternalTranieesAnswerAction",
                schema: "ExamTraining",
                newName: "ExamInternalTranieesAnswerAction",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesQuestionAnswer",
                schema: "ExamTraining",
                newName: "ExamExternalTranieesQuestionAnswer",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "ExamExternalTranieesAnswerAction",
                schema: "ExamTraining",
                newName: "ExamExternalTranieesAnswerAction",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "CourseAdvertisements",
                schema: "ExamTraining",
                newName: "CourseAdvertisements",
                newSchema: "ExamTrainingCourses");

            migrationBuilder.RenameTable(
                name: "AdvertisementsCourses",
                schema: "ExamTraining",
                newName: "AdvertisementsCourses",
                newSchema: "ExamTrainingCourses");
        }
    }
}
