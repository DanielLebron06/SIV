using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Auditoria
{
    public class LogAuditoriaDTO
    {
        public Guid Id { get; set; }

        public string Actor { get; set; } = string.Empty;

        public Modulo Modulo { get; set; }

        public TipoAccion TipoAccion { get; set; }

        public string Resultado { get; set; } = string.Empty;

        public Guid? EntidadAfectadaId { get; set; }

        public string? EntidadAfectadaDescripcion { get; set; }

        public DateTime FechaHora { get; set; }
    }
}
``