using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Competitions.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinCount = table.Column<int>(type: "int", nullable: true),
                    MaxCount = table.Column<int>(type: "int", nullable: true),
                    AcceptedExtention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CompetitorsType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitorsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniversityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<int>(type: "int", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    CommercialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentsUniversity = table.Column<bool>(type: "bit", nullable: true),
                    UniversityApprovalDoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTeam = table.Column<bool>(type: "bit", nullable: true),
                    IsLecturer = table.Column<bool>(type: "bit", nullable: true),
                    ProfileDoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCandidated = table.Column<bool>(type: "bit", nullable: true),
                    CandidatedBy = table.Column<int>(type: "int", nullable: true),
                    CandidatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCompleteAttachFile = table.Column<bool>(type: "bit", nullable: true),
                    ComplateAttachDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Competitors_CompetitorsType",
                        column: x => x.TypeId,
                        principalTable: "CompetitorsType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    CompetitorId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Attachments_AttachmentType",
                        column: x => x.TypeId,
                        principalTable: "AttachmentType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Attachments_Competitors",
                        column: x => x.CompetitorId,
                        principalTable: "Competitors",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Competitors",
                        column: x => x.CompetitorId,
                        principalTable: "Competitors",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CompetitorId",
                table: "Attachments",
                column: "CompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TypeId",
                table: "Attachments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_TypeId",
                table: "Competitors",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_CompetitorId",
                table: "TeamMembers",
                column: "CompetitorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "AttachmentType");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "CompetitorsType");
        }
    }
}
