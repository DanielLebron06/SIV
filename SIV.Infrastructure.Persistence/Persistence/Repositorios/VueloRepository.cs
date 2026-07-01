using Microsoft.EntityFrameworkCore;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class VueloRepository: BaseRepository<Vuelo>, IVueloRepository
    {
        public VueloRepository(SIVDbContext context) : base(context) { }

        public async Task<Vuelo?> BuscarPorNumeroVuelo(string numeroVuelo)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.NumeroVuelo == numeroVuelo);
        }

        public async Task<List<Vuelo>> BuscarPorAerolinea(Guid aerolineaId)
        {
            return await _dbSet
                .Where(v => v.AerolineaId == aerolineaId)
                .ToListAsync();
        }

        public async Task<List<Vuelo>> BuscarPorAeropuerto(Guid aeropuertoId)
        {
            return await _dbSet
                .Where(v => v.AeropuertoOrigenId == aeropuertoId ||
                            v.AeropuertoDestinoId == aeropuertoId)
                .ToListAsync();
        }

        public async Task<List<Vuelo>> BuscarConFiltros(FiltrosVuelos filtros)
        {
            var query = _dbSet.AsQueryable();

            if (filtros.AerolineaId.HasValue)
                query = query.Where(v => v.AerolineaId == filtros.AerolineaId);

            if (filtros.AeropuertoOrigenId.HasValue)
                query = query.Where(v => v.AeropuertoOrigenId == filtros.AeropuertoOrigenId);

            if (filtros.AeropuertoDestinoId.HasValue)
                query = query.Where(v => v.AeropuertoDestinoId == filtros.AeropuertoDestinoId);

            if (filtros.Estado.HasValue)
                query = query.Where(v => v.EstadoActual == filtros.Estado);

            if (filtros.Fecha.HasValue)
                query = query.Where(v =>
                    v.SalidaPlanificada.Date == filtros.Fecha.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<List<Vuelo>> BuscarPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Where(v => v.CreadoEn >= fechaInicio &&
                            v.CreadoEn <= fechaFin)
                .ToListAsync();
        }
    }

}
