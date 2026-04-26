using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class feedbacks_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Feedback");

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsActive",
            //    schema: "dbo",
            //    table: "MajorLookups",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsViewStatistic",
            //    schema: "DynamicForm",
            //    table: "Forms",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "Rate",
            //    schema: "Survey",
            //    table: "DataSource",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "CronSettings",
            //    schema: "dbo",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CronTypeId = table.Column<int>(type: "int", nullable: true),
            //        Emails = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EntityId = table.Column<int>(type: "int", nullable: true),
            //        SurveyId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //        UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        ReferenceId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CronSettings", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_CronSettings_References",
            //            column: x => x.ReferenceId,
            //            principalTable: "References",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_CronSettings_UsersCreatedBy",
            //            column: x => x.CreatedBy,
            //            principalTable: "Users",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_CronSettings_UsersUpdatedBy",
            //            column: x => x.UpdatedBy,
            //            principalTable: "Users",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_Entity_CronSettings",
            //            column: x => x.EntityId,
            //            principalTable: "Entities",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_Survey_CronSettings",
            //            column: x => x.SurveyId,
            //            principalSchema: "Survey",
            //            principalTable: "Surveys",
            //            principalColumn: "Id");
            //    });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                schema: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Feedbacks_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Feedbacks_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Feedbacks_UsersDeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Feedbacks_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            //migrationBuilder.CreateTable(
            //    name: "FormValueViewStatistic",
            //    schema: "DynamicForm",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FormValueId = table.Column<int>(type: "int", nullable: true),
            //        EntityId = table.Column<int>(type: "int", nullable: true),
            //        TextValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<int>(type: "int", nullable: true),
            //        UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserReferenceId = table.Column<int>(type: "int", nullable: true),
            //        UserReferenceNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserReferenceNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ViewDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FormValueViewStatistic", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Recommendations",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EntityId = table.Column<int>(type: "int", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //        UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        ReferenceId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Recommendations", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Recommendations_Entities_EntityId",
            //            column: x => x.EntityId,
            //            principalTable: "Entities",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_Recommendations_References",
            //            column: x => x.ReferenceId,
            //            principalTable: "References",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_Recommendations_UsersCreatedBy",
            //            column: x => x.CreatedBy,
            //            principalTable: "Users",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_Recommendations_UsersUpdatedBy",
            //            column: x => x.UpdatedBy,
            //            principalTable: "Users",
            //            principalColumn: "ID");
            //    });

            migrationBuilder.CreateTable(
                name: "FeedbacksAnswerAction",
                schema: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedbacksId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    IsHelpful = table.Column<bool>(type: "bit", nullable: true),
                    ItemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbacksAnswerAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbacksAnswerAction_Feedbacks",
                        column: x => x.FeedbacksId,
                        principalSchema: "Feedback",
                        principalTable: "Feedbacks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeedbacksDataSource",
                schema: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasNote = table.Column<bool>(type: "bit", nullable: true),
                    IsHelpful = table.Column<bool>(type: "bit", nullable: true),
                    FeedbacksId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    LessAverageId = table.Column<int>(type: "int", nullable: true),
                    AverageId = table.Column<int>(type: "int", nullable: true),
                    AboveAverageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbacksDataSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbacksDataSource_Feedbacks",
                        column: x => x.FeedbacksId,
                        principalSchema: "Feedback",
                        principalTable: "Feedbacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedbacksDataSource_Recommendations_AboveAverageId",
                        column: x => x.AboveAverageId,
                        principalTable: "Recommendations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedbacksDataSource_Recommendations_AverageId",
                        column: x => x.AverageId,
                        principalTable: "Recommendations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedbacksDataSource_Recommendations_LessAverageId",
                        column: x => x.LessAverageId,
                        principalTable: "Recommendations",
                        principalColumn: "Id");
                });

            //migrationBuilder.CreateTable(
            //    name: "QuestionsRecommendations",
            //    schema: "Survey",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        LessAverageId = table.Column<int>(type: "int", nullable: true),
            //        AverageId = table.Column<int>(type: "int", nullable: true),
            //        AboveAverageId = table.Column<int>(type: "int", nullable: true),
            //        QuestionId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_QuestionsRecommendations", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_QuestionRecommendationAboveAverages",
            //            column: x => x.AboveAverageId,
            //            principalTable: "Recommendations",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_QuestionRecommendationAverages",
            //            column: x => x.AverageId,
            //            principalTable: "Recommendations",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_QuestionRecommendationLessAverages",
            //            column: x => x.LessAverageId,
            //            principalTable: "Recommendations",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_QuestionsRecommendations_Questions",
            //            column: x => x.QuestionId,
            //            principalSchema: "Survey",
            //            principalTable: "Questions",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "FeedbacksAnswers",
                schema: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbacksDataSourceId = table.Column<int>(type: "int", nullable: true),
                    FeedbacksAnswerActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbacksAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbacksAnswers_FeedbacksAnswerAction",
                        column: x => x.FeedbacksAnswerActionId,
                        principalSchema: "Feedback",
                        principalTable: "FeedbacksAnswerAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedbacksAnswers_FeedbacksDataSource",
                        column: x => x.FeedbacksDataSourceId,
                        principalSchema: "Feedback",
                        principalTable: "FeedbacksDataSource",
                        principalColumn: "Id");
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CronSettings_CreatedBy",
            //    schema: "dbo",
            //    table: "CronSettings",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CronSettings_EntityId",
            //    schema: "dbo",
            //    table: "CronSettings",
            //    column: "EntityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CronSettings_ReferenceId",
            //    schema: "dbo",
            //    table: "CronSettings",
            //    column: "ReferenceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CronSettings_SurveyId",
            //    schema: "dbo",
            //    table: "CronSettings",
            //    column: "SurveyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CronSettings_UpdatedBy",
            //    schema: "dbo",
            //    table: "CronSettings",
            //    column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CreatedBy",
                schema: "Feedback",
                table: "Feedbacks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DeletedBy",
                schema: "Feedback",
                table: "Feedbacks",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_EntityId",
                schema: "Feedback",
                table: "Feedbacks",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReferenceId",
                schema: "Feedback",
                table: "Feedbacks",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UpdatedBy",
                schema: "Feedback",
                table: "Feedbacks",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksAnswerAction_FeedbacksId",
                schema: "Feedback",
                table: "FeedbacksAnswerAction",
                column: "FeedbacksId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksAnswers_FeedbacksAnswerActionId",
                schema: "Feedback",
                table: "FeedbacksAnswers",
                column: "FeedbacksAnswerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksAnswers_FeedbacksDataSourceId",
                schema: "Feedback",
                table: "FeedbacksAnswers",
                column: "FeedbacksDataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksDataSource_AboveAverageId",
                schema: "Feedback",
                table: "FeedbacksDataSource",
                column: "AboveAverageId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksDataSource_AverageId",
                schema: "Feedback",
                table: "FeedbacksDataSource",
                column: "AverageId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksDataSource_FeedbacksId",
                schema: "Feedback",
                table: "FeedbacksDataSource",
                column: "FeedbacksId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksDataSource_LessAverageId",
                schema: "Feedback",
                table: "FeedbacksDataSource",
                column: "LessAverageId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_QuestionsRecommendations_AboveAverageId",
            //    schema: "Survey",
            //    table: "QuestionsRecommendations",
            //    column: "AboveAverageId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_QuestionsRecommendations_AverageId",
            //    schema: "Survey",
            //    table: "QuestionsRecommendations",
            //    column: "AverageId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_QuestionsRecommendations_LessAverageId",
            //    schema: "Survey",
            //    table: "QuestionsRecommendations",
            //    column: "LessAverageId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_QuestionsRecommendations_QuestionId",
            //    schema: "Survey",
            //    table: "QuestionsRecommendations",
            //    column: "QuestionId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Recommendations_CreatedBy",
            //    table: "Recommendations",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Recommendations_EntityId",
            //    table: "Recommendations",
            //    column: "EntityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Recommendations_ReferenceId",
            //    table: "Recommendations",
            //    column: "ReferenceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Recommendations_UpdatedBy",
            //    table: "Recommendations",
            //    column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "CronSettings",
            //    schema: "dbo");

            migrationBuilder.DropTable(
                name: "FeedbacksAnswers",
                schema: "Feedback");

            //migrationBuilder.DropTable(
            //    name: "FormValueViewStatistic",
            //    schema: "DynamicForm");

            //migrationBuilder.DropTable(
            //    name: "QuestionsRecommendations",
            //    schema: "Survey");

            migrationBuilder.DropTable(
                name: "FeedbacksAnswerAction",
                schema: "Feedback");

            migrationBuilder.DropTable(
                name: "FeedbacksDataSource",
                schema: "Feedback");

            migrationBuilder.DropTable(
                name: "Feedbacks",
                schema: "Feedback");

            //migrationBuilder.DropTable(
            //    name: "Recommendations");

            //migrationBuilder.DropColumn(
            //    name: "IsActive",
            //    schema: "dbo",
            //    table: "MajorLookups");

            //migrationBuilder.DropColumn(
            //    name: "IsViewStatistic",
            //    schema: "DynamicForm",
            //    table: "Forms");

            //migrationBuilder.DropColumn(
            //    name: "Rate",
            //    schema: "Survey",
            //    table: "DataSource");
        }
    }
}
