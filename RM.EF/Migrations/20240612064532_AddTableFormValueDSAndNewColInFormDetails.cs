#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddTableFormValueDSAndNewColInFormDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BooleanValue",
                schema: "DynamicForm",
                table: "FormValueDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeValue",
                schema: "DynamicForm",
                table: "FormValueDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InputTypeId",
                schema: "DynamicForm",
                table: "FormValueDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NumericValue",
                schema: "DynamicForm",
                table: "FormValueDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormValueDataSource",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormValueId = table.Column<int>(type: "int", nullable: true),
                    FormDataSourceId = table.Column<int>(type: "int", nullable: true),
                    InputKey = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormValueDataSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormValueDataSource_FormInputs_InputKey",
                        column: x => x.InputKey,
                        principalSchema: "DynamicForm",
                        principalTable: "FormInputs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormValueDataSource_FormValue_FormValueId",
                        column: x => x.FormValueId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormValueDataSource_FormsDataSource_FormDataSourceId",
                        column: x => x.FormDataSourceId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormsDataSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormValueDetails_InputTypeId",
                schema: "DynamicForm",
                table: "FormValueDetails",
                column: "InputTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValueDataSource_FormDataSourceId",
                schema: "DynamicForm",
                table: "FormValueDataSource",
                column: "FormDataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValueDataSource_FormValueId",
                schema: "DynamicForm",
                table: "FormValueDataSource",
                column: "FormValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValueDataSource_InputKey",
                schema: "DynamicForm",
                table: "FormValueDataSource",
                column: "InputKey");

            migrationBuilder.AddForeignKey(
                name: "FK_FormValueDetails_InputsType_InputTypeId",
                schema: "DynamicForm",
                table: "FormValueDetails",
                column: "InputTypeId",
                principalSchema: "DynamicForm",
                principalTable: "InputsType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormValueDetails_InputsType_InputTypeId",
                schema: "DynamicForm",
                table: "FormValueDetails");

            migrationBuilder.DropTable(
                name: "FormValueDataSource",
                schema: "DynamicForm");

            migrationBuilder.DropIndex(
                name: "IX_FormValueDetails_InputTypeId",
                schema: "DynamicForm",
                table: "FormValueDetails");

            migrationBuilder.DropColumn(
                name: "BooleanValue",
                schema: "DynamicForm",
                table: "FormValueDetails");

            migrationBuilder.DropColumn(
                name: "DateTimeValue",
                schema: "DynamicForm",
                table: "FormValueDetails");

            migrationBuilder.DropColumn(
                name: "InputTypeId",
                schema: "DynamicForm",
                table: "FormValueDetails");

            migrationBuilder.DropColumn(
                name: "NumericValue",
                schema: "DynamicForm",
                table: "FormValueDetails");
        }
    }
}
