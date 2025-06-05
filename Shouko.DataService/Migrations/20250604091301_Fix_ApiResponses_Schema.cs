using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Fix_ApiResponses_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_InteractionId1",
                table: "ApiResponses");

            migrationBuilder.RenameColumn(
                name: "InteractionId1",
                table: "ApiResponses",
                newName: "DiscordInteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_ApiResponses_InteractionId1",
                table: "ApiResponses",
                newName: "IX_ApiResponses_DiscordInteractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_DiscordInteractionId",
                table: "ApiResponses",
                column: "DiscordInteractionId",
                principalTable: "DiscordInteractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_DiscordInteractionId",
                table: "ApiResponses");

            migrationBuilder.RenameColumn(
                name: "DiscordInteractionId",
                table: "ApiResponses",
                newName: "InteractionId1");

            migrationBuilder.RenameIndex(
                name: "IX_ApiResponses_DiscordInteractionId",
                table: "ApiResponses",
                newName: "IX_ApiResponses_InteractionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_InteractionId1",
                table: "ApiResponses",
                column: "InteractionId1",
                principalTable: "DiscordInteractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
