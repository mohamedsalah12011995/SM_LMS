using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class CronSettings_subEntityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "EmailBody",
            //    schema: "WorkFlow",
            //    table: "EngineActionJobRole",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsSendEmail",
            //    schema: "WorkFlow",
            //    table: "EngineActionJobRole",
            //    type: "bit",
            //    nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubEntityId",
                schema: "dbo",
                table: "CronSettings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CronSettings_SubEntityId",
                schema: "dbo",
                table: "CronSettings",
                column: "SubEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_CronSubEntitySettings",
                schema: "dbo",
                table: "CronSettings",
                column: "SubEntityId",
                principalTable: "Entities",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entity_CronSubEntitySettings",
                schema: "dbo",
                table: "CronSettings");

            migrationBuilder.DropIndex(
                name: "IX_CronSettings_SubEntityId",
                schema: "dbo",
                table: "CronSettings");

            //migrationBuilder.DropColumn(
            //    name: "EmailBody",
            //    schema: "WorkFlow",
            //    table: "EngineActionJobRole");

            //migrationBuilder.DropColumn(
            //    name: "IsSendEmail",
            //    schema: "WorkFlow",
            //    table: "EngineActionJobRole");

            migrationBuilder.DropColumn(
                name: "SubEntityId",
                schema: "dbo",
                table: "CronSettings");
        }
    }
}
