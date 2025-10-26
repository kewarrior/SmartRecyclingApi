using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRecyclingApi.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoNovaTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ref_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Tipo_Pedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status_Pedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Criacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reciclagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ref_Utilizador = table.Column<int>(type: "int", nullable: false),
                    MatVidro = table.Column<double>(type: "float", nullable: false),
                    MatPlastico = table.Column<double>(type: "float", nullable: false),
                    MatPapel = table.Column<double>(type: "float", nullable: false),
                    data_Reciclagem = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reciclagem", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Reciclagem");
        }
    }
}
