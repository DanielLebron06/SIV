using System;

namespace SIV.Domain.Entities
{
    public class SeguimientoVuelo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public Guid VueloId { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
        public DateTime? FechaFin { get; set; } // Nullable mientras el seguimiento esté activo
        public Usuario? Usuario { get; set; }
        public Vuelo? Vuelo { get; set; }
    }
}