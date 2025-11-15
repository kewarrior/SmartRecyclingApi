using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRecyclingApi.Migrations
{
    /// <inheritdoc />
    public partial class Utilizadoresrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Utilizadores");
        }
    }
}
