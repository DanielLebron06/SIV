using System;

namespace SIV.Application.Domain.Entities
{
    public class LogAuditoria
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Actor { get; set; } = string.Empty; // Exigido por el flujo tabular del SRS/SAD
        public string Modulo { get; set; } = string.Empty;
        public string TipoAccion { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    }
}