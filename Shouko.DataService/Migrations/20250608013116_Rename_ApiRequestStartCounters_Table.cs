using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Rename_ApiRequestStartCounters_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiRequestStartCounters",
                table: "ApiRequestStartCounters");

            migrationBuilder.RenameTable(
                name: "ApiRequestStartCounters",
                newName: "ApiRequestLimitCounters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiRequestLimitCounters",
                table: "ApiRequestLimitCounters",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiRequestLimitCounters",
                table: "ApiRequestLimitCounters");

            migrationBuilder.RenameTable(
                name: "ApiRequestLimitCounters",
                newName: "ApiRequestStartCounters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiRequestStartCounters",
                table: "ApiRequestStartCounters",
                column: "Id");
        }
    }
}
