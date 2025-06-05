using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Rename_ApiResponses_ApiPromptType_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PromptTokenDetails",
                table: "ApiResponses",
                newName: "ApiPromptType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiPromptType",
                table: "ApiResponses",
                newName: "PromptTokenDetails");
        }
    }
}
