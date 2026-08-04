using Microsoft.EntityFrameworkCore;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Emuns;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class VueloRepository: BaseRepository<Vuelo>, IVueloRepository
    {
        public VueloRepository(SIVDbContext context) : base(context) { }

        public async Task<Vuelo?> BuscarPorNumeroVuelo(string numeroVuelo)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(v => v.NumeroVuelo == numeroVuelo);
        }

        public async Task<bool> ExisteDuplicadoAsync(string numeroVuelo, Guid aerolineaId, Guid aeropuertoOrigenId, Guid aeropuertoDestinoId, DateTimeOffset fechaSalida)
        {
            var fechaInicio = fechaSalida.Date;
            var fechaFin = fechaInicio.AddDays(1);
            return await _dbSet.AsNoTracking().AnyAsync(v =>
                v.NumeroVuelo == numeroVuelo &&
                v.AerolineaId == aerolineaId &&
                v.AeropuertoOrigenId == aeropuertoOrigenId &&
                v.AeropuertoDestinoId == aeropuertoDestinoId &&
                v.SalidaPlanificada >= fechaInicio &&
                v.SalidaPlanificada < fechaFin);
        }

        public async Task<List<Vuelo>> BuscarPorAerolinea(Guid aerolineaId)
        {
            return await _dbSet.AsNoTracking()
                .Where(v => v.AerolineaId == aerolineaId)
                .ToListAsync();
        }

        public async Task<List<Vuelo>> BuscarPorAeropuerto(Guid aeropuertoId)
        {
            var origen = _dbSet.AsNoTracking().Where(v => v.AeropuertoOrigenId == aeropuertoId);
            var destino = _dbSet.AsNoTracking().Where(v => v.AeropuertoDestinoId == aeropuertoId);

            return await origen.Union(destino).ToListAsync();
        }

        public async Task<bool> ExistenVuelosActivosPorAerolineaAsync(Guid aerolineaId)
        {
            return await _dbSet.AsNoTracking().AnyAsync(v =>
                v.AerolineaId == aerolineaId &&
                v.EstadoActual != EstadoVuelo.Cancelado &&
                v.EstadoActual != EstadoVuelo.Completado);
        }

        public async Task<bool> ExistenVuelosActivosPorAeropuertoAsync(Guid aeropuertoId)
        {
            return await _dbSet.AsNoTracking().AnyAsync(v =>
                (v.AeropuertoOrigenId == aeropuertoId || v.AeropuertoDestinoId == aeropuertoId) &&
                v.EstadoActual != EstadoVuelo.Cancelado &&
                v.EstadoActual != EstadoVuelo.Completado);
        }

        public async Task<List<Vuelo>> BuscarConFiltros(FiltrosVuelos filtros)
        {
            var query = _dbSet.AsNoTracking()
                .Include(v => v.Aerolinea)
                .Include(v => v.AeropuertoOrigen)
                .Include(v => v.AeropuertoDestino)
                .AsQueryable();

            if (filtros.AerolineaId.HasValue)
                query = query.Where(v => v.AerolineaId == filtros.AerolineaId);

            if (filtros.AeropuertoOrigenId.HasValue)
                query = query.Where(v => v.AeropuertoOrigenId == filtros.AeropuertoOrigenId);

            if (filtros.AeropuertoDestinoId.HasValue)
                query = query.Where(v => v.AeropuertoDestinoId == filtros.AeropuertoDestinoId);

            if (filtros.Estado.HasValue)
                query = query.Where(v => v.EstadoActual == filtros.Estado);

            if (filtros.Fecha.HasValue)
            {
                var fechaInicio = filtros.Fecha.Value.Date;
                var fechaFin = fechaInicio.AddDays(1);
                query = query.Where(v => v.SalidaPlanificada >= fechaInicio && v.SalidaPlanificada < fechaFin);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Vuelo>> BuscarParaFidsAsync(FiltrosFids filtros)
        {
            var query = _dbSet.AsNoTracking()
                .Include(v => v.Aerolinea)
                .Include(v => v.AeropuertoOrigen)
                .Include(v => v.AeropuertoDestino)
                .AsQueryable();

            if (filtros.Estado.HasValue)
                query = query.Where(v => v.EstadoActual == filtros.Estado);

            if (filtros.AerolineaId.HasValue)
                query = query.Where(v => v.AerolineaId == filtros.AerolineaId);

            if (!string.IsNullOrWhiteSpace(filtros.AeropuertoCodigo))
            {
                var codigo = filtros.AeropuertoCodigo.Trim().ToUpperInvariant();

                if (filtros.TipoPantalla == TipoPantallaFids.Salidas)
                {
                    query = query.Where(v => v.AeropuertoOrigen != null && v.AeropuertoOrigen.CodigoIATA == codigo);
                }
                else if (filtros.TipoPantalla == TipoPantallaFids.Llegadas)
                {
                    query = query.Where(v => v.AeropuertoDestino != null && v.AeropuertoDestino.CodigoIATA == codigo);
                }
                else
                {
                    query = query.Where(v =>
                        (v.AeropuertoOrigen != null && v.AeropuertoOrigen.CodigoIATA == codigo) ||
                        (v.AeropuertoDestino != null && v.AeropuertoDestino.CodigoIATA == codigo));
                }
            }

            if (filtros.RangoHoras.HasValue)
            {
                var ahora = DateTimeOffset.Now;
                var fin = ahora + filtros.RangoHoras.Value;
                query = query.Where(v =>
                    (v.SalidaPlanificada >= ahora && v.SalidaPlanificada <= fin) ||
                    (v.LlegadaPlanificada >= ahora && v.LlegadaPlanificada <= fin));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Vuelo>> BuscarPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet.AsNoTracking()
                .Where(v => v.CreadoEn >= fechaInicio &&
                            v.CreadoEn <= fechaFin)
                .ToListAsync();
        }

        public async Task<Vuelo?> GetVueloConDetallesAsync(Guid id)
        {
            return await _dbSet
                .Include(v => v.Aerolinea)
                .Include(v => v.AeropuertoOrigen)
                .Include(v => v.AeropuertoDestino)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }

}
