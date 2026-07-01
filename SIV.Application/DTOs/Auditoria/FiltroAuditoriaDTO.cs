using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Auditoria
{
    public class FiltroAuditoriaDTO
    {
        public string? Actor { get; set; }

        public Modulo? Modulo { get; set; }

        public TipoAccion? TipoAccion { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }
    }
}