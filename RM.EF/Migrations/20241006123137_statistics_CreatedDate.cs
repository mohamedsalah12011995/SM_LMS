#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class statistics_CreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "InteractionStatistics",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "InteractionStatistics");
        }
    }
}
