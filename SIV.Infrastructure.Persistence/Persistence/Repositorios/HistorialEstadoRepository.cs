using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class HistorialEstadoRepository : BaseRepository<HistorialEstado>, IHistorialEstadoRepository
    {
        public HistorialEstadoRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<List<HistorialEstado>> ObtenerPorVueloAsync(Guid vueloId)
        {
            return await _dbSet
                .Where(h => h.VueloId == vueloId)
                .OrderByDescending(h => h.FechaTransicion)
                .ToListAsync();
        }
    }

}
