using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Add_Additional_ApiRequestLimitCounter_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApiPromptType",
                table: "ApiRequestStartCounters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApiType",
                table: "ApiRequestStartCounters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LimitCounterType",
                table: "ApiRequestStartCounters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiPromptType",
                table: "ApiRequestStartCounters");

            migrationBuilder.DropColumn(
                name: "ApiType",
                table: "ApiRequestStartCounters");

            migrationBuilder.DropColumn(
                name: "LimitCounterType",
                table: "ApiRequestStartCounters");
        }
    }
}
