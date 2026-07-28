using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIV.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoCatalogoAereo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aerolineas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoIATA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aerolineas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aeropuertos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoIATA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aeropuertos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_AerolineaId",
                table: "Vuelos",
                column: "AerolineaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_AeropuertoDestinoId",
                table: "Vuelos",
                column: "AeropuertoDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_AeropuertoOrigenId",
                table: "Vuelos",
                column: "AeropuertoOrigenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vuelos_Aerolineas_AerolineaId",
                table: "Vuelos",
                column: "AerolineaId",
                principalTable: "Aerolineas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vuelos_Aeropuertos_AeropuertoDestinoId",
                table: "Vuelos",
                column: "AeropuertoDestinoId",
                principalTable: "Aeropuertos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vuelos_Aeropuertos_AeropuertoOrigenId",
                table: "Vuelos",
                column: "AeropuertoOrigenId",
                principalTable: "Aeropuertos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vuelos_Aerolineas_AerolineaId",
                table: "Vuelos");

            migrationBuilder.DropForeignKey(
                name: "FK_Vuelos_Aeropuertos_AeropuertoDestinoId",
                table: "Vuelos");

            migrationBuilder.DropForeignKey(
                name: "FK_Vuelos_Aeropuertos_AeropuertoOrigenId",
                table: "Vuelos");

            migrationBuilder.DropTable(
                name: "Aerolineas");

            migrationBuilder.DropTable(
                name: "Aeropuertos");

            migrationBuilder.DropIndex(
                name: "IX_Vuelos_AerolineaId",
                table: "Vuelos");

            migrationBuilder.DropIndex(
                name: "IX_Vuelos_AeropuertoDestinoId",
                table: "Vuelos");

            migrationBuilder.DropIndex(
                name: "IX_Vuelos_AeropuertoOrigenId",
                table: "Vuelos");
        }
    }
}
