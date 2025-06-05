using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Nullable_DiscordInteractionId_InApi_Response : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_DiscordInteractionId",
                table: "ApiResponses");

            migrationBuilder.DropColumn(
                name: "InteractionId",
                table: "ApiResponses");

            migrationBuilder.AlterColumn<int>(
                name: "DiscordInteractionId",
                table: "ApiResponses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_DiscordInteractionId",
                table: "ApiResponses",
                column: "DiscordInteractionId",
                principalTable: "DiscordInteractions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_DiscordInteractionId",
                table: "ApiResponses");

            migrationBuilder.AlterColumn<int>(
                name: "DiscordInteractionId",
                table: "ApiResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InteractionId",
                table: "ApiResponses",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiResponses_DiscordInteractions_DiscordInteractionId",
                table: "ApiResponses",
                column: "DiscordInteractionId",
                principalTable: "DiscordInteractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
