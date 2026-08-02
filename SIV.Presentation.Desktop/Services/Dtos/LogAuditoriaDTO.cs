using System;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class LogAuditoriaDTO
    {
        public Guid Id { get; set; }
        public string Actor { get; set; } = string.Empty;
        public Modulo Modulo { get; set; }
        public TipoAccion TipoAccion { get; set; }
        public string Resultado { get; set; } = string.Empty;
        public Guid? EntidadAfectadaId { get; set; }
        public string EntidadAfectadaDescripcion { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }

    public class FiltroAuditoriaDTO
    {
        public string Actor { get; set; } = string.Empty;
        public Modulo? Modulo { get; set; }
        public TipoAccion? TipoAccion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
