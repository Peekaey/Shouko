using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Add_ApiResponse_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InteractionId = table.Column<long>(type: "bigint", nullable: false),
                    InteractionId1 = table.Column<int>(type: "integer", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApiType = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    ResponseText = table.Column<string>(type: "text", nullable: false),
                    ResponseId = table.Column<string>(type: "text", nullable: false),
                    PromptTokenDetails = table.Column<int>(type: "integer", nullable: false),
                    CandidatesTokenCount = table.Column<int>(type: "integer", nullable: false),
                    PromptTokenCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiResponses_DiscordInteractions_InteractionId1",
                        column: x => x.InteractionId1,
                        principalTable: "DiscordInteractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiResponses_InteractionId1",
                table: "ApiResponses",
                column: "InteractionId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiResponses");
        }
    }
}
