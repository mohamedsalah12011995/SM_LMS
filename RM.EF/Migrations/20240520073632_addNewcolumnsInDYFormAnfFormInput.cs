using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addNewcolumnsInDYFormAnfFormInput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckApplicationNo",
                schema: "DynamicForm",
                table: "Forms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CheckPersonalData",
                schema: "DynamicForm",
                table: "Forms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonEditableForm",
                schema: "DynamicForm",
                table: "Forms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseIntegration",
                schema: "DynamicForm",
                table: "Forms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InputUseIntegration",
                schema: "DynamicForm",
                table: "FormInputs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Length",
                schema: "DynamicForm",
                table: "FormInputs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInExport",
                schema: "DynamicForm",
                table: "FormInputs",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckApplicationNo",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "CheckPersonalData",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "NonEditableForm",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "UseIntegration",
                schema: "DynamicForm",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "InputUseIntegration",
                schema: "DynamicForm",
                table: "FormInputs");

            migrationBuilder.DropColumn(
                name: "Length",
                schema: "DynamicForm",
                table: "FormInputs");

            migrationBuilder.DropColumn(
                name: "ShowInExport",
                schema: "DynamicForm",
                table: "FormInputs");
        }
    }
}
