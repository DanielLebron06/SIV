using SIV.Application.Common;

namespace SIV.Application.Domain.Entities
{
    public class Vuelo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Llaves foráneas explícitas del modelo relacional del SAD
        public Guid AerolineaId { get; set; }
        public Guid AeropuertoOrigenId { get; set; }
        public Guid AeropuertoDestinoId { get; set; }

        public string NumeroVuelo { get; set; } = string.Empty;
        public EstadoVuelo EstadoActual { get; set; } = EstadoVuelo.Programado; // EstadoActual según diccionario

        // Uso obligatorio de datetimeoffset para la precisión de vuelos
        public DateTimeOffset SalidaPlanificada { get; set; }
        public DateTimeOffset LlegadaPlanificada { get; set; }

        // Propiedades Nullable (SÍ aceptan nulos en el SAD)
        public DateTimeOffset? SalidaActualizada { get; set; }
        public DateTimeOffset? LlegadaActualizada { get; set; }
        public string? PuertaEmbarque { get; set; }

        // Metadatos de auditoría requeridos en la Tabla Vuelos
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public Guid CreadoPorId { get; set; }

        // Relaciones de Navegación exigidas por los Bounded Contexts
        public List<CambioOperativo> CambiosOperativos { get; set; } = new List<CambioOperativo>();
        public List<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>(); // Exigido en sección 5.1
        public List<SeguimientoVuelo> Seguidores { get; set; } = new List<SeguimientoVuelo>();
        public List<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    }
}