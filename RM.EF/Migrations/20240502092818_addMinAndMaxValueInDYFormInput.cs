#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class addMinAndMaxValueInDYFormInput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxValue",
                schema: "DynamicForm",
                table: "FormInputs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinValue",
                schema: "DynamicForm",
                table: "FormInputs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxValue",
                schema: "DynamicForm",
                table: "FormInputs");

            migrationBuilder.DropColumn(
                name: "MinValue",
                schema: "DynamicForm",
                table: "FormInputs");
        }
    }
}
