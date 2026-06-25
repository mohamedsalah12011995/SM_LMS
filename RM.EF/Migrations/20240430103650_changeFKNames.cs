#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class changeFKNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "CreatedByNavigation",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "UpdatedByNavigation",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "CreatedByNavigation",
                schema: "Permits",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "DeletedByNavigation",
                schema: "Permits",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "UpdatedByNavigation",
                schema: "Permits",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "CreatedByNavigation",
                table: "PublishEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatedBy_Form",
                schema: "DynamicForm",
                table: "Forms",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UpdatedBy_Form",
                schema: "DynamicForm",
                table: "Forms",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatedBy_Project",
                schema: "Permits",
                table: "Projects",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DeletedBy_Project",
                schema: "Permits",
                table: "Projects",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UpdatedBy_Project",
                schema: "Permits",
                table: "Projects",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatedBy_PublishEntities",
                table: "PublishEntities",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatedBy_Form",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_UpdatedBy_Form",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatedBy_Project",
                schema: "Permits",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_DeletedBy_Project",
                schema: "Permits",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UpdatedBy_Project",
                schema: "Permits",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatedBy_PublishEntities",
                table: "PublishEntities");

            migrationBuilder.AddForeignKey(
                name: "CreatedByNavigation",
                schema: "DynamicForm",
                table: "Forms",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "UpdatedByNavigation",
                schema: "DynamicForm",
                table: "Forms",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "CreatedByNavigation",
                schema: "Permits",
                table: "Projects",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "DeletedByNavigation",
                schema: "Permits",
                table: "Projects",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "UpdatedByNavigation",
                schema: "Permits",
                table: "Projects",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "CreatedByNavigation",
                table: "PublishEntities",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
