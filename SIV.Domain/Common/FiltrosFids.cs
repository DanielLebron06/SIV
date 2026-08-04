using SIV.Domain.Emuns;

namespace SIV.Domain.Common
{
    public class FiltrosFids
    {
        public TipoPantallaFids TipoPantalla { get; set; } = TipoPantallaFids.General;
        public string? AeropuertoCodigo { get; set; }
        public EstadoVuelo? Estado { get; set; }
        public Guid? AerolineaId { get; set; }
        public TimeSpan? RangoHoras { get; set; }
    }
}
