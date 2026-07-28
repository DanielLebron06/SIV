using Microsoft.EntityFrameworkCore;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class LogAuditoriaRepository
        : BaseRepository<LogAuditoria>, ILogAuditoriaRepository
    {
        public LogAuditoriaRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<List<LogAuditoria>> BuscarConFiltrosAsync(
            string? actor,
            Modulo? modulo,
            TipoAccion? tipoAccion,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(actor))
            {
                query = query.Where(l => l.Actor.Contains(actor));
            }

            if (modulo.HasValue)
            {
                query = query.Where(l => l.Modulo == modulo.Value);
            }

            if (tipoAccion.HasValue)
            {
                query = query.Where(l => l.TipoAccion == tipoAccion.Value);
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(l => l.FechaHora >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(l => l.FechaHora <= fechaFin.Value);
            }

            return await query
                .OrderByDescending(l => l.FechaHora)
                .ToListAsync();
        }
    }
}