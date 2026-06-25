#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class ideaLastActionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastActionId",
                schema: "Innovation",
                table: "Ideas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_LastActionId",
                schema: "Innovation",
                table: "Ideas",
                column: "LastActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_IdeaActions_LastActionId",
                schema: "Innovation",
                table: "Ideas",
                column: "LastActionId",
                principalSchema: "Innovation",
                principalTable: "IdeaActions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_IdeaActions_LastActionId",
                schema: "Innovation",
                table: "Ideas");

            migrationBuilder.DropIndex(
                name: "IX_Ideas_LastActionId",
                schema: "Innovation",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "LastActionId",
                schema: "Innovation",
                table: "Ideas");
        }
    }
}
