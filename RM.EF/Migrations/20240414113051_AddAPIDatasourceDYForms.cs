using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddAPIDatasourceDYForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "StepNavigation",
                schema: "Permits",
                table: "PermitActions");



            migrationBuilder.CreateTable(
                name: "APIDataSources",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayMthod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParamTypeIsObject = table.Column<bool>(type: "bit", nullable: false),
                    ParameterDataListMethodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParameterControlTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnChangeAPIMethodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnChangeParameterName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APIDataSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APIDataSourceParamObjectDetails",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APIDataSourceId = table.Column<int>(type: "int", nullable: false),
                    ParamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayParamNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayParamNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParameterDataListMethodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParameterControlTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APIDataSourceParamObjectDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_APIDataSourceParamObjectDetails_APIDataSources_APIDataSourceId",
                        column: x => x.APIDataSourceId,
                        principalSchema: "DynamicForm",
                        principalTable: "APIDataSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermitActions_StepId",
                schema: "Permits",
                table: "PermitActions",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_APIDataSourceParamObjectDetails_APIDataSourceId",
                schema: "DynamicForm",
                table: "APIDataSourceParamObjectDetails",
                column: "APIDataSourceId");

            migrationBuilder.AddForeignKey(
                name: "StepNavigation",
                schema: "Permits",
                table: "PermitActions",
                column: "StepId",
                principalSchema: "Permits",
                principalTable: "FlowStepper",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "StepNavigation",
                schema: "Permits",
                table: "PermitActions");

            migrationBuilder.DropTable(
                name: "APIDataSourceParamObjectDetails",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "APIDataSources",
                schema: "DynamicForm");

            migrationBuilder.DropIndex(
                name: "IX_PermitActions_StepId",
                schema: "Permits",
                table: "PermitActions");


            migrationBuilder.AddForeignKey(
                name: "StepNavigation",
                schema: "Permits",
                table: "PermitActions",
                column: "UpdatedBy",
                principalSchema: "Permits",
                principalTable: "FlowStepper",
                principalColumn: "Id");
        }
    }
}
