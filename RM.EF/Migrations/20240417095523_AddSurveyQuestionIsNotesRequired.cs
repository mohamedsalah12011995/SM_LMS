using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyQuestionIsNotesRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Innovation");

            migrationBuilder.AddColumn<bool>(
                name: "IsNoteRequired",
                schema: "Survey",
                table: "Questions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Survey",
                table: "QuestionAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ideas",
                schema: "Innovation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<long>(type: "bigint", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdeaAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdeaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdeaExist = table.Column<bool>(type: "bit", nullable: true),
                    IsShow = table.Column<bool>(type: "bit", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: true),
                    ToReference = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Capability = table.Column<bool>(type: "bit", nullable: true),
                    NeedsBudget = table.Column<bool>(type: "bit", nullable: true),
                    Feasibility = table.Column<bool>(type: "bit", nullable: true),
                    NeedsPeriod = table.Column<int>(type: "int", nullable: true),
                    AgreeCount = table.Column<int>(type: "int", nullable: true),
                    DisAgreeCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ideas_MajorLookupsCategory",
                        column: x => x.Category,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_MajorLookupsPriority",
                        column: x => x.Priority,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_MajorLookupsStatus",
                        column: x => x.Status,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_MajorLookupsType",
                        column: x => x.Type,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_ToReferences",
                        column: x => x.ToReference,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_UsersActivatedBy",
                        column: x => x.ActivatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_UsersDeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ideas_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IdeasCompetentAuthorities",
                schema: "Innovation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeasCompetentAuthorities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IdeasCompetentAuthorities_References",
                        column: x => x.ReferenceID,
                        principalTable: "References",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IdeaActions",
                schema: "Innovation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaActions_Ideas",
                        column: x => x.IdeaId,
                        principalSchema: "Innovation",
                        principalTable: "Ideas",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IdeaActions_MajorLookupsType",
                        column: x => x.Type,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IdeaActions_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IdeaComments",
                schema: "Innovation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommenterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepliedBy = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAgreeTerms = table.Column<bool>(type: "bit", nullable: true),
                    IdeaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaComments_Idea",
                        column: x => x.IdeaId,
                        principalSchema: "Innovation",
                        principalTable: "Ideas",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IdeaComments_UsersApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IdeaComments_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IdeaComments_UsersRepliedBy",
                        column: x => x.RepliedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaActions_CreatedBy",
                schema: "Innovation",
                table: "IdeaActions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaActions_IdeaId",
                schema: "Innovation",
                table: "IdeaActions",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaActions_Type",
                schema: "Innovation",
                table: "IdeaActions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaComments_ApprovedBy",
                schema: "Innovation",
                table: "IdeaComments",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaComments_CreatedBy",
                schema: "Innovation",
                table: "IdeaComments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaComments_IdeaId",
                schema: "Innovation",
                table: "IdeaComments",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaComments_RepliedBy",
                schema: "Innovation",
                table: "IdeaComments",
                column: "RepliedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_ActivatedBy",
                schema: "Innovation",
                table: "Ideas",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_Category",
                schema: "Innovation",
                table: "Ideas",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_CreatedBy",
                schema: "Innovation",
                table: "Ideas",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_DeletedBy",
                schema: "Innovation",
                table: "Ideas",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_Priority",
                schema: "Innovation",
                table: "Ideas",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_ReferenceId",
                schema: "Innovation",
                table: "Ideas",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_Status",
                schema: "Innovation",
                table: "Ideas",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_ToReference",
                schema: "Innovation",
                table: "Ideas",
                column: "ToReference");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_Type",
                schema: "Innovation",
                table: "Ideas",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_UpdatedBy",
                schema: "Innovation",
                table: "Ideas",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdeasCompetentAuthorities_ReferenceID",
                schema: "Innovation",
                table: "IdeasCompetentAuthorities",
                column: "ReferenceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdeaActions",
                schema: "Innovation");

            migrationBuilder.DropTable(
                name: "IdeaComments",
                schema: "Innovation");

            migrationBuilder.DropTable(
                name: "IdeasCompetentAuthorities",
                schema: "Innovation");

            migrationBuilder.DropTable(
                name: "Ideas",
                schema: "Innovation");

            migrationBuilder.DropColumn(
                name: "IsNoteRequired",
                schema: "Survey",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Survey",
                table: "QuestionAnswers");
        }
    }
}
