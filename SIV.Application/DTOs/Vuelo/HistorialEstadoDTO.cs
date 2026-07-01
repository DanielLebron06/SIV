using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Vuelo
{
    public class HistorialEstadoDTO
    {
        public Guid Id { get; set; }
        public EstadoVuelo Estado { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}