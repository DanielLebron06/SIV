using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Vuelo
{
    public class DatosVueloDTO
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public Guid AerolineaId { get; set; }
        public Guid AeropuertoOrigenId { get; set; }
        public Guid AeropuertoDestinoId { get; set; }
        public string AerolineaNombre { get; set; } = string.Empty;
        public string AeropuertoOrigenIATA { get; set; } = string.Empty;
        public string AeropuertoDestinoIATA { get; set; } = string.Empty;
        public EstadoVuelo EstadoActual { get; set; }
        public string? PuertaEmbarque { get; set; }
        public DateTimeOffset FechaSalidaProgramada { get; set; }
        public DateTimeOffset FechaLlegadaProgramada { get; set; }
    }
}