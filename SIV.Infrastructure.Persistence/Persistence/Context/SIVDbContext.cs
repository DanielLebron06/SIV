using Microsoft.EntityFrameworkCore;
using SIV.Application.Domain.Entities;

namespace SIV.Infrastructure.Persistence.Context
{
    public class SIVDbContext : DbContext
    {
        public SIVDbContext(DbContextOptions<SIVDbContext> options) : base(options)
        {
        }

        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CambioOperativo> CambiosOperativos { get; set; }
        public DbSet<HistorialEstado> HistorialEstados { get; set; }
        public DbSet<SeguimientoVuelo> SeguimientosVuelos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo preciso según el Diccionario de Datos del SAD
            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroVuelo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.EstadoActual).HasConversion<string>().HasMaxLength(50); // Almacena el texto del Enum en DB
                entity.Property(e => e.PuertaEmbarque).HasMaxLength(10);
            });

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

            // Relación de Seguimiento
            modelBuilder.Entity<SeguimientoVuelo>()
                .HasOne(s => s.Usuario)
                .WithMany(u => u.Seguimientos)
                .HasForeignKey(s => s.UsuarioId);

            modelBuilder.Entity<SeguimientoVuelo>()
                .HasOne(s => s.Vuelo)
                .WithMany(v => v.Seguidores)
                .HasForeignKey(s => s.VueloId);
        }
    }
}