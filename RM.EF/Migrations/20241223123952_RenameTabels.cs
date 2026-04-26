using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class RenameTabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopicTrainingCourses",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "Topics",
                schema: "Exams");

            migrationBuilder.CreateTable(
                name: "CourseAdvertisements",
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
                    table.PrimaryKey("PK_CourseAdvertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAdvertisements_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CourseAdvertisements_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CourseAdvertisements_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CourseAdvertisements_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CourseAdvertisementsCourses",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAdvertisementId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAdvertisementsCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAdvertisementsCourses_CourseAdvertisements_CourseAdvertisementId",
                        column: x => x.CourseAdvertisementId,
                        principalSchema: "Exams",
                        principalTable: "CourseAdvertisements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseAdvertisementsCourses_TrainingCourses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Exams",
                        principalTable: "TrainingCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisements_CreatedBy",
                schema: "Exams",
                table: "CourseAdvertisements",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisements_DeletedBy",
                schema: "Exams",
                table: "CourseAdvertisements",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisements_ReferenceId",
                schema: "Exams",
                table: "CourseAdvertisements",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisements_UpdatedBy",
                schema: "Exams",
                table: "CourseAdvertisements",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisementsCourses_CourseAdvertisementId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "CourseAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAdvertisementsCourses_CourseId",
                schema: "Exams",
                table: "CourseAdvertisementsCourses",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAdvertisementsCourses",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "CourseAdvertisements",
                schema: "Exams");

            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "TopicTrainingCourses",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: true),
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
        }
    }
}
