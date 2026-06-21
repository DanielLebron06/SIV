using SIV.Domain.Emuns;

namespace SIV.Domain.Entities
{
    public class HistorialEstado
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VueloId { get; set; }
        public EstadoVuelo Estado { get; set; }
        public DateTime FechaTransicion { get; set; } = DateTime.UtcNow;
        public Vuelo? Vuelo { get; set; }
    }
}