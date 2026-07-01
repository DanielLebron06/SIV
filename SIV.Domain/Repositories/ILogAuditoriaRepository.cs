using SIV.Domain.Emuns;
using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface ILogAuditoriaRepository : IBaseRepository<LogAuditoria>
    {
        Task<List<LogAuditoria>> BuscarConFiltrosAsync(
            string? actor,
            Modulo? modulo,
            TipoAccion? tipoAccion,
            DateTime? fechaInicio,
            DateTime? fechaFin);
    }
}