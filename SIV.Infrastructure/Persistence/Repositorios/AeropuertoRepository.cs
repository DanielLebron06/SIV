using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class AeropuertoRepository
    : BaseRepository<Aeropuerto>, IAeropuertoRepository
    {
        public AeropuertoRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<Aeropuerto?> BuscarPorCodigoAsync(string codigoAeropuerto)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.CodigoIATA == codigoAeropuerto && a.Activo);
        }

        public async Task<bool> ExistePorCodigoAsync(string codigoAeropuerto)
        {
            return await _dbSet
                .AnyAsync(a => a.CodigoIATA == codigoAeropuerto);
        }

        public async Task<List<Aeropuerto>> ObtenerActivosAsync()
        {
            return await _dbSet
                .Where(a => a.Activo)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }
    }
}
