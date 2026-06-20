using Microsoft.EntityFrameworkCore;
using SIV.Application.Domain.Entities;

namespace SIV.Infrastructure.Persistence
{
    public class SIVDbContext : DbContext
    {
        public SIVDbContext(DbContextOptions<SIVDbContext> options) : base(options)
        {
        }

        // Entidades Principales del Ciclo de Vida
        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CambioOperativo> CambiosOperativos { get; set; }
        public DbSet<HistorialEstado> HistorialEstados { get; set; }
        public DbSet<SeguimientoVuelo> SeguimientosVuelos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }

        // Entidades del Módulo de Catálogo Aeroportuario (RFCAT)
        public DbSet<Aerolinea> Aerolineas { get; set; }
        public DbSet<Aeropuerto> Aeropuertos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo preciso según el Diccionario de Datos del SAD
            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroVuelo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.EstadoActual).HasConversion<string>().HasMaxLength(50);
                entity.Property(e => e.PuertaEmbarque).HasMaxLength(10);

                // ==========================================
                // REQUISITO DEL SAD: RELACIONES DE CATÁLOGOS
                // ==========================================

                entity.HasOne<Aerolinea>()
                      .WithMany()
                      .HasForeignKey(e => e.AerolineaId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Un Vuelo tiene un Aeropuerto de Origen
                entity.HasOne<Aeropuerto>()
                      .WithMany()
                      .HasForeignKey(e => e.AeropuertoOrigenId)
                      .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Aeropuerto>()
                      .WithMany()
                      .HasForeignKey(e => e.AeropuertoDestinoId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Relación Vuelo -> HistorialEstado (1 a Muchos)
                modelBuilder.Entity<HistorialEstado>()
                    .HasOne(h => h.Vuelo)
                    .WithMany(v => v.HistorialEstados)
                    .HasForeignKey(h => h.VueloId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación Vuelo -> CambiosOperativos (1 a Muchos)
                modelBuilder.Entity<CambioOperativo>()
                    .HasOne(c => c.Vuelo)
                    .WithMany(v => v.CambiosOperativos)
                    .HasForeignKey(c => c.VueloId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación de Seguimiento (Muchos a Muchos Explícita)
                modelBuilder.Entity<SeguimientoVuelo>()
                    .HasOne(s => s.Usuario)
                    .WithMany(u => u.Seguimientos)
                    .HasForeignKey(s => s.UsuarioId);

                modelBuilder.Entity<SeguimientoVuelo>()
                    .HasOne(s => s.Vuelo)
                    .WithMany(v => v.Seguidores)
                    .HasForeignKey(s => s.VueloId);

                // Relación Notificaciones (1 a Muchos con Vuelo y Usuario)
                modelBuilder.Entity<Notificacion>()
                    .HasOne(n => n.Vuelo)
                    .WithMany(v => v.Notificaciones)
                    .HasForeignKey(n => n.VueloId);
            });
        }
    }
}