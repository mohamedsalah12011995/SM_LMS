using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class Complaints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemEntityId",
                schema: "Contact",
                table: "ContactUs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                schema: "Contact",
                table: "ContactUs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_ItemEntityId",
                schema: "Contact",
                table: "ContactUs",
                column: "ItemEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_Entities_ItemEntityId",
                schema: "Contact",
                table: "ContactUs",
                column: "ItemEntityId",
                principalTable: "Entities",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactUs_Entities_ItemEntityId",
                schema: "Contact",
                table: "ContactUs");

            migrationBuilder.DropIndex(
                name: "IX_ContactUs_ItemEntityId",
                schema: "Contact",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "ItemEntityId",
                schema: "Contact",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "Contact",
                table: "ContactUs");
        }
    }
}
