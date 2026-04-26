using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class CreateExamStandaloneTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Topics_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Topics_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Topics_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TrainingCourses",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    LoginRequired = table.Column<bool>(type: "bit", nullable: false),
                    HasCertificate = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCourses_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourses_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourses_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourses_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourses_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TopicTrainingCourses",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicTrainingCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicTrainingCourses_Topics_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "Exams",
                        principalTable: "Topics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopicTrainingCourses_TrainingCourses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingCourseSchedule",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    DepartmentReferenceId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCourseSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_References_DepartmentReferenceId",
                        column: x => x.DepartmentReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_TrainingCourses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrainingCourseSchedule_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExternalCourseTraniees",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingCourseScheduleId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradeType = table.Column<int>(type: "int", nullable: false),
                    GradeYear = table.Column<int>(type: "int", nullable: true),
                    OnWaitingList = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalCourseTraniees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalCourseTraniees_TrainingCourseSchedule_TrainingCourseScheduleId",
                        column: x => x.TrainingCourseScheduleId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourseSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternalCourseExams",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingCourseScheduleId = table.Column<int>(type: "int", nullable: true),
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
                    IsAttendExam = table.Column<bool>(type: "bit", nullable: true),
                    AnswerTotalTime = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<double>(type: "float", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalCourseExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalCourseExams_TrainingCourseSchedule_TrainingCourseScheduleId",
                        column: x => x.TrainingCourseScheduleId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternalCourseExams_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_InternalCourseExams_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_InternalCourseExams_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "InternalCourseTrainees",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingCourseScheduleId = table.Column<int>(type: "int", nullable: true),
                    TraineeId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    TraineeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalCourseTrainees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalCourseTrainees_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_InternalCourseTrainees_TrainingCourseSchedule_TrainingCourseScheduleId",
                        column: x => x.TrainingCourseScheduleId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternalCourseTrainees_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCourseTraniees_TrainingCourseScheduleId",
                schema: "Exams",
                table: "ExternalCourseTraniees",
                column: "TrainingCourseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_CreatedBy",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_DeletedBy",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_TrainingCourseScheduleId",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "TrainingCourseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseExams_UpdatedBy",
                schema: "Exams",
                table: "InternalCourseExams",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_ReferenceId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_TraineeId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalCourseTrainees_TrainingCourseScheduleId",
                schema: "Exams",
                table: "InternalCourseTrainees",
                column: "TrainingCourseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CreatedBy",
                schema: "Exams",
                table: "Topics",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DeletedBy",
                schema: "Exams",
                table: "Topics",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ReferenceId",
                schema: "Exams",
                table: "Topics",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_UpdatedBy",
                schema: "Exams",
                table: "Topics",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TopicTrainingCourses_CourseId",
                schema: "Exams",
                table: "TopicTrainingCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicTrainingCourses_TopicId",
                schema: "Exams",
                table: "TopicTrainingCourses",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourses_CreatedBy",
                schema: "Exams",
                table: "TrainingCourses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourses_DeletedBy",
                schema: "Exams",
                table: "TrainingCourses",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourses_EntityId",
                schema: "Exams",
                table: "TrainingCourses",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourses_ReferenceId",
                schema: "Exams",
                table: "TrainingCourses",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourses_UpdatedBy",
                schema: "Exams",
                table: "TrainingCourses",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_CourseId",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_CreatedBy",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_DeletedBy",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_DepartmentReferenceId",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "DepartmentReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_ExamId",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_ReferenceId",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourseSchedule_UpdatedBy",
                schema: "Exams",
                table: "TrainingCourseSchedule",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalCourseTraniees",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "InternalCourseExams",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "InternalCourseTrainees",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "TopicTrainingCourses",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "TrainingCourseSchedule",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "Topics",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "TrainingCourses",
                schema: "Exams");
        }
    }
}
