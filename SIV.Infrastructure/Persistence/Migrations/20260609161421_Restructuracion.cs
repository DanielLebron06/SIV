using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIV.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Restructuracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CambiosOperativos_Usuarios_IdUsuarioResponsable",
                table: "CambiosOperativos");

            migrationBuilder.DropForeignKey(
                name: "FK_CambiosOperativos_Vuelos_IdVuelo",
                table: "CambiosOperativos");

            migrationBuilder.DropTable(
                name: "Catalogos");

            migrationBuilder.DropTable(
                name: "HistorialNotificaciones");

            migrationBuilder.DropTable(
                name: "Seguimientos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vuelos",
                table: "Vuelos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CambiosOperativos",
                table: "CambiosOperativos");

            migrationBuilder.DropIndex(
                name: "IX_CambiosOperativos_IdUsuarioResponsable",
                table: "CambiosOperativos");

            migrationBuilder.DropIndex(
                name: "IX_CambiosOperativos_IdVuelo",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "IdVuelo",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "Aerolinea",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AeropuertoDestino",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AeropuertoOrigen",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "Gate",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "LlegadaProgramada",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CorreoElectronico",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdCambio",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "EstadoAnterior",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "IdUsuarioResponsable",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "Justificacion",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "NuevoEstado",
                table: "CambiosOperativos");

            migrationBuilder.RenameColumn(
                name: "SalidaProgramada",
                table: "Vuelos",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "IdVuelo",
                table: "CambiosOperativos",
                newName: "TipoCambio");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "CambiosOperativos",
                newName: "Timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroVuelo",
                table: "Vuelos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "EstadoActual",
                table: "Vuelos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Vuelos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AerolineaId",
                table: "Vuelos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AeropuertoDestinoId",
                table: "Vuelos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AeropuertoOrigenId",
                table: "Vuelos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPorId",
                table: "Vuelos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LlegadaActualizada",
                table: "Vuelos",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LlegadaPlanificada",
                table: "Vuelos",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "PuertaEmbarque",
                table: "Vuelos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SalidaActualizada",
                table: "Vuelos",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SalidaPlanificada",
                table: "Vuelos",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<int>(
                name: "Rol",
                table: "Usuarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Usuarios",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CambiosOperativos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "CambiosOperativos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "VueloId",
                table: "CambiosOperativos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vuelos",
                table: "Vuelos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CambiosOperativos",
                table: "CambiosOperativos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HistorialEstados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VueloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaTransicion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstados_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Actor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoAccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resultado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VueloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leida = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeguimientosVuelos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VueloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientosVuelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeguimientosVuelos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeguimientosVuelos_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CambiosOperativos_VueloId",
                table: "CambiosOperativos",
                column: "VueloId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstados_VueloId",
                table: "HistorialEstados",
                column: "VueloId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_VueloId",
                table: "Notificaciones",
                column: "VueloId");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosVuelos_UsuarioId",
                table: "SeguimientosVuelos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosVuelos_VueloId",
                table: "SeguimientosVuelos",
                column: "VueloId");

            migrationBuilder.AddForeignKey(
                name: "FK_CambiosOperativos_Vuelos_VueloId",
                table: "CambiosOperativos",
                column: "VueloId",
                principalTable: "Vuelos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CambiosOperativos_Vuelos_VueloId",
                table: "CambiosOperativos");

            migrationBuilder.DropTable(
                name: "HistorialEstados");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "SeguimientosVuelos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vuelos",
                table: "Vuelos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CambiosOperativos",
                table: "CambiosOperativos");

            migrationBuilder.DropIndex(
                name: "IX_CambiosOperativos_VueloId",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AerolineaId",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AeropuertoDestinoId",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AeropuertoOrigenId",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "CreadoPorId",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "LlegadaActualizada",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "LlegadaPlanificada",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "PuertaEmbarque",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "SalidaActualizada",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "SalidaPlanificada",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "VueloId",
                table: "CambiosOperativos");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "Vuelos",
                newName: "SalidaProgramada");

            migrationBuilder.RenameColumn(
                name: "TipoCambio",
                table: "CambiosOperativos",
                newName: "IdVuelo");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "CambiosOperativos",
                newName: "FechaRegistro");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroVuelo",
                table: "Vuelos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "EstadoActual",
                table: "Vuelos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "IdVuelo",
                table: "Vuelos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Aerolinea",
                table: "Vuelos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AeropuertoDestino",
                table: "Vuelos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AeropuertoOrigen",
                table: "Vuelos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gate",
                table: "Vuelos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LlegadaProgramada",
                table: "Vuelos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CorreoElectronico",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdCambio",
                table: "CambiosOperativos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "EstadoAnterior",
                table: "CambiosOperativos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioResponsable",
                table: "CambiosOperativos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Justificacion",
                table: "CambiosOperativos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NuevoEstado",
                table: "CambiosOperativos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vuelos",
                table: "Vuelos",
                column: "IdVuelo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "IdUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CambiosOperativos",
                table: "CambiosOperativos",
                column: "IdCambio");

            migrationBuilder.CreateTable(
                name: "Catalogos",
                columns: table => new
                {
                    IdCatalogo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogos", x => x.IdCatalogo);
                });

            migrationBuilder.CreateTable(
                name: "HistorialNotificaciones",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCambioOperativo = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Medio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialNotificaciones", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_HistorialNotificaciones_CambiosOperativos_IdCambioOperativo",
                        column: x => x.IdCambioOperativo,
                        principalTable: "CambiosOperativos",
                        principalColumn: "IdCambio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialNotificaciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seguimientos",
                columns: table => new
                {
                    IdSeguimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdVuelo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguimientos", x => x.IdSeguimiento);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Vuelos_IdVuelo",
                        column: x => x.IdVuelo,
                        principalTable: "Vuelos",
                        principalColumn: "IdVuelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CambiosOperativos_IdUsuarioResponsable",
                table: "CambiosOperativos",
                column: "IdUsuarioResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosOperativos_IdVuelo",
                table: "CambiosOperativos",
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
                name: "IX_Seguimientos_IdUsuario",
                table: "Seguimientos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_IdVuelo",
                table: "Seguimientos",
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
        }
    }
}
