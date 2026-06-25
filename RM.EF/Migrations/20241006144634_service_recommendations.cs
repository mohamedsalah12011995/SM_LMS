#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class service_recommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AboveAverageId",
                table: "GovServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageId",
                table: "GovServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessAverageId",
                table: "GovServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AboveAverageId",
                table: "Eservices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageId",
                table: "Eservices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessAverageId",
                table: "Eservices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_AboveAverageId",
                table: "GovServices",
                column: "AboveAverageId");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_AverageId",
                table: "GovServices",
                column: "AverageId");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_LessAverageId",
                table: "GovServices",
                column: "LessAverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_AboveAverageId",
                table: "Eservices",
                column: "AboveAverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_AverageId",
                table: "Eservices",
                column: "AverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_LessAverageId",
                table: "Eservices",
                column: "LessAverageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_Recommendations_AboveAverageId",
                table: "Eservices",
                column: "AboveAverageId",
                principalTable: "Recommendations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_Recommendations_AverageId",
                table: "Eservices",
                column: "AverageId",
                principalTable: "Recommendations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_Recommendations_LessAverageId",
                table: "Eservices",
                column: "LessAverageId",
                principalTable: "Recommendations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GovServices_Recommendations_AboveAverageId",
                table: "GovServices",
                column: "AboveAverageId",
                principalTable: "Recommendations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GovServices_Recommendations_AverageId",
                table: "GovServices",
                column: "AverageId",
                principalTable: "Recommendations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GovServices_Recommendations_LessAverageId",
                table: "GovServices",
                column: "LessAverageId",
                principalTable: "Recommendations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eservices_Recommendations_AboveAverageId",
                table: "Eservices");

            migrationBuilder.DropForeignKey(
                name: "FK_Eservices_Recommendations_AverageId",
                table: "Eservices");

            migrationBuilder.DropForeignKey(
                name: "FK_Eservices_Recommendations_LessAverageId",
                table: "Eservices");

            migrationBuilder.DropForeignKey(
                name: "FK_GovServices_Recommendations_AboveAverageId",
                table: "GovServices");

            migrationBuilder.DropForeignKey(
                name: "FK_GovServices_Recommendations_AverageId",
                table: "GovServices");

            migrationBuilder.DropForeignKey(
                name: "FK_GovServices_Recommendations_LessAverageId",
                table: "GovServices");

            migrationBuilder.DropIndex(
                name: "IX_GovServices_AboveAverageId",
                table: "GovServices");

            migrationBuilder.DropIndex(
                name: "IX_GovServices_AverageId",
                table: "GovServices");

            migrationBuilder.DropIndex(
                name: "IX_GovServices_LessAverageId",
                table: "GovServices");

            migrationBuilder.DropIndex(
                name: "IX_Eservices_AboveAverageId",
                table: "Eservices");

            migrationBuilder.DropIndex(
                name: "IX_Eservices_AverageId",
                table: "Eservices");

            migrationBuilder.DropIndex(
                name: "IX_Eservices_LessAverageId",
                table: "Eservices");

            migrationBuilder.DropColumn(
                name: "AboveAverageId",
                table: "GovServices");

            migrationBuilder.DropColumn(
                name: "AverageId",
                table: "GovServices");

            migrationBuilder.DropColumn(
                name: "LessAverageId",
                table: "GovServices");

            migrationBuilder.DropColumn(
                name: "AboveAverageId",
                table: "Eservices");

            migrationBuilder.DropColumn(
                name: "AverageId",
                table: "Eservices");

            migrationBuilder.DropColumn(
                name: "LessAverageId",
                table: "Eservices");
        }
    }
}
