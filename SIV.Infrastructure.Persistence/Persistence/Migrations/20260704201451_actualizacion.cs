using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIV.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class actualizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TipoAccion",
                table: "LogsAuditoria",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Modulo",
                table: "LogsAuditoria",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionEntidad",
                table: "LogsAuditoria",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntidadAfectadaId",
                table: "LogsAuditoria",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioResponsableId",
                table: "CambiosOperativos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Aeropuertos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Aerolineas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DescripcionEntidad",
                table: "LogsAuditoria");

            migrationBuilder.DropColumn(
                name: "EntidadAfectadaId",
                table: "LogsAuditoria");

            migrationBuilder.DropColumn(
                name: "UsuarioResponsableId",
                table: "CambiosOperativos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Aeropuertos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Aerolineas");

            migrationBuilder.AlterColumn<string>(
                name: "TipoAccion",
                table: "LogsAuditoria",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Modulo",
                table: "LogsAuditoria",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
