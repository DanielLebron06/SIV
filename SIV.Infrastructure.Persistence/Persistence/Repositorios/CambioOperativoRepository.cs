using SIV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class CambioOperativoRepository
    : BaseRepository<CambioOperativo>, ICambioOperativoRepository
    {
        public CambioOperativoRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<List<CambioOperativo>> BuscarPorVueloAsync(Guid vueloId)
        {
            return await _dbSet
                .Where(c => c.VueloId == vueloId)
                .OrderByDescending(c => c.Timestamp)
                .ToListAsync();
        }
    }
}
