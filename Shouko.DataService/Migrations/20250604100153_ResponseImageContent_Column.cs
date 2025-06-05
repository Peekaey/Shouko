using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class ResponseImageContent_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponseText",
                table: "ApiResponses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ResponseImageContent",
                table: "ApiResponses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseImageContent",
                table: "ApiResponses");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseText",
                table: "ApiResponses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
