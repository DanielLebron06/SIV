using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class AerolineaRepository : BaseRepository<Aerolinea>, IAerolineaRepository
    {
        public AerolineaRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<Aerolinea?> BuscarPorCodigoAsync(string codigoAerolinea)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.CodigoIATA == codigoAerolinea && a.Activo);
        }

        public async Task<List<Aerolinea>> ObtenerActivosAsync()
        {
            return await _dbSet
                .Where(a => a.Activo)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }
    }
}
