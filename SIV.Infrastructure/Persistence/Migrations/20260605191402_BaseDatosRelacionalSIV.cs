using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIV.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BaseDatosRelacionalSIV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GateInformativa",
                table: "Vuelos",
                newName: "Gate");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_IdUsuario",
                table: "Seguimientos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_IdVuelo",
                table: "Seguimientos",
                column: "IdVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialNotificaciones_IdCambioOperativo",
                table: "HistorialNotificaciones",
                column: "IdCambioOperativo");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialNotificaciones_IdUsuario",
                table: "HistorialNotificaciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosOperativos_IdUsuarioResponsable",
                table: "CambiosOperativos",
                column: "IdUsuarioResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosOperativos_IdVuelo",
                table: "CambiosOperativos",
                column: "IdVuelo");

            migrationBuilder.AddForeignKey(
                name: "FK_CambiosOperativos_Usuarios_IdUsuarioResponsable",
                table: "CambiosOperativos",
                column: "IdUsuarioResponsable",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CambiosOperativos_Vuelos_IdVuelo",
                table: "CambiosOperativos",
                column: "IdVuelo",
                principalTable: "Vuelos",
                principalColumn: "IdVuelo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialNotificaciones_CambiosOperativos_IdCambioOperativo",
                table: "HistorialNotificaciones",
                column: "IdCambioOperativo",
                principalTable: "CambiosOperativos",
                principalColumn: "IdCambio",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialNotificaciones_Usuarios_IdUsuario",
                table: "HistorialNotificaciones",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_Usuarios_IdUsuario",
                table: "Seguimientos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_Vuelos_IdVuelo",
                table: "Seguimientos",
                column: "IdVuelo",
                principalTable: "Vuelos",
                principalColumn: "IdVuelo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CambiosOperativos_Usuarios_IdUsuarioResponsable",
                table: "CambiosOperativos");

            migrationBuilder.DropForeignKey(
                name: "FK_CambiosOperativos_Vuelos_IdVuelo",
                table: "CambiosOperativos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialNotificaciones_CambiosOperativos_IdCambioOperativo",
                table: "HistorialNotificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialNotificaciones_Usuarios_IdUsuario",
                table: "HistorialNotificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_Usuarios_IdUsuario",
                table: "Seguimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_Vuelos_IdVuelo",
                table: "Seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_Seguimientos_IdUsuario",
                table: "Seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_Seguimientos_IdVuelo",
                table: "Seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_HistorialNotificaciones_IdCambioOperativo",
                table: "HistorialNotificaciones");

            migrationBuilder.DropIndex(
                name: "IX_HistorialNotificaciones_IdUsuario",
                table: "HistorialNotificaciones");

            migrationBuilder.DropIndex(
                name: "IX_CambiosOperativos_IdUsuarioResponsable",
                table: "CambiosOperativos");

            migrationBuilder.DropIndex(
                name: "IX_CambiosOperativos_IdVuelo",
                table: "CambiosOperativos");

            migrationBuilder.RenameColumn(
                name: "Gate",
                table: "Vuelos",
                newName: "GateInformativa");
        }
    }
}
