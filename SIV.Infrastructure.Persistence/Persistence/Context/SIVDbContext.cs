using Microsoft.EntityFrameworkCore;
using SIV.Application.Domain.Entities;
using SIV.Domain.Entities;

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
        public DbSet<Seguimiento> Seguimientos { get; set; }
        public DbSet<HistorialNotificacion> HistorialNotificaciones { get; set; }
        public DbSet<Catalogo> Catalogos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo simple: Le decimos a EF Core que almacene el enum de estados como texto
            modelBuilder.Entity<Vuelo>()
                .Property(e => e.EstadoActual)
                .HasConversion<string>();

            modelBuilder.Entity<HistorialNotificacion>()
       .HasOne(h => h.Usuario)
       .WithMany() // Se deja vacío porque Usuario no tiene una lista de notificaciones
       .HasForeignKey(h => h.IdUsuario)
       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}