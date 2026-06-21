using SIV.Domain.Emuns;

namespace SIV.Domain.Entities
{
    public class Vuelo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AerolineaId { get; set; }
        public Guid AeropuertoOrigenId { get; set; }
        public Guid AeropuertoDestinoId { get; set; }

        public string NumeroVuelo { get; set; } = string.Empty;
        public EstadoVuelo EstadoActual { get; set; } = EstadoVuelo.Programado;

        public DateTimeOffset SalidaPlanificada { get; set; }
        public DateTimeOffset LlegadaPlanificada { get; set; }

        public DateTimeOffset? SalidaActualizada { get; set; }
        public DateTimeOffset? LlegadaActualizada { get; set; }
        public string? PuertaEmbarque { get; set; }

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public Guid CreadoPorId { get; set; }

        public List<CambioOperativo> CambiosOperativos { get; set; } = new List<CambioOperativo>();
        public List<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>(); // Exigido en sección 5.1
        public List<SeguimientoVuelo> Seguidores { get; set; } = new List<SeguimientoVuelo>();
        public List<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    }
}