using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Reportes
{
    public class ReporteCambioOperativoDTO
    {
        public Guid Id { get; set; }

        public Guid VueloId { get; set; }

        public string NumeroVuelo { get; set; } = string.Empty;

        public TipoCambio TipoCambio { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public DateTime FechaCambio { get; set; }

        public Guid OperadorId { get; set; }
    }
}