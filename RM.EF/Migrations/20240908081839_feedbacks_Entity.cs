#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class feedbacks_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                schema: "Feedback",
                table: "FeedbacksAnswerAction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbacksAnswerAction_EntityId",
                schema: "Feedback",
                table: "FeedbacksAnswerAction",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbacksAnswerAction_Entity",
                schema: "Feedback",
                table: "FeedbacksAnswerAction",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbacksAnswerAction_Entity",
                schema: "Feedback",
                table: "FeedbacksAnswerAction");

            migrationBuilder.DropIndex(
                name: "IX_FeedbacksAnswerAction_EntityId",
                schema: "Feedback",
                table: "FeedbacksAnswerAction");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "Feedback",
                table: "FeedbacksAnswerAction");
        }
    }
}
