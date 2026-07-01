
using SIV.Domain.Emuns;

namespace SIV.Domain.Common
{
    public class FiltrosVuelos
    {
        public Guid? AerolineaId { get; set; }
        public Guid? AeropuertoOrigenId { get; set; }
        public Guid? AeropuertoDestinoId { get; set; }
        public DateTimeOffset? Fecha { get; set; }
        public EstadoVuelo? Estado { get; set; }
    }
}
