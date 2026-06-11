using System;

namespace SIV.Application.Domain.Entities
{
    public class SeguimientoVuelo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public Guid VueloId { get; set; }

        // Estructura del PeriodoSeguimiento exigido en el SAD
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
        public DateTime? FechaFin { get; set; } // Nullable mientras el seguimiento esté activo

        public Usuario? Usuario { get; set; }
        public Vuelo? Vuelo { get; set; }
    }
}