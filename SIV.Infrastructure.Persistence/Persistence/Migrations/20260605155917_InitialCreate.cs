using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIV.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vuelos",
                columns: table => new
                {
                    IdVuelo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroVuelo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Aerolinea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AeropuetoOrigen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AeropuetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoActual = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SalidaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LlegadaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GateInformativa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vuelos", x => x.IdVuelo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vuelos");
        }
    }
}
