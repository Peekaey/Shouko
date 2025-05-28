using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shouko.DataService.Migrations
{
    /// <inheritdoc />
    public partial class Add_Input_Column_ApiResponse_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InputText",
                table: "ApiResponses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputText",
                table: "ApiResponses");
        }
    }
}
