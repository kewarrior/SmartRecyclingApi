using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRecyclingApi.Migrations
{
    /// <inheritdoc />
    public partial class CriarBancoDeDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    morada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigo_postal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<int>(type: "int", nullable: false),
                    pontos = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    adesao = table.Column<bool>(type: "bit", nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
