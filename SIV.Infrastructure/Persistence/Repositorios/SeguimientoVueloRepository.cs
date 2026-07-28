

using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class SeguimientoVueloRepository
    : BaseRepository<SeguimientoVuelo>, ISeguimientoVueloRepository
    {
        public SeguimientoVueloRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<List<SeguimientoVuelo>> BuscarPorUsuario(Guid usuarioId)
        {
            return await _dbSet
                .Where(s => s.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<SeguimientoVuelo>> BuscarActivosPorUsuario(Guid usuarioId)
        {
            return await _dbSet
                .Where(s => s.UsuarioId == usuarioId && s.FechaFin == null)
                .ToListAsync();
        }

        public async Task<List<SeguimientoVuelo>> BuscarInactivosPorUsuario(Guid usuarioId)
        {
            return await _dbSet
                .Where(s => s.UsuarioId == usuarioId && s.FechaFin != null)
                .ToListAsync();
        }

        public async Task<bool> ExisteSeguimiento(Guid usuarioId, Guid vueloId)
        {
            return await _dbSet
                .AnyAsync(s => s.UsuarioId == usuarioId
                            && s.VueloId == vueloId
                            && s.FechaFin == null);
        }

        public async Task<SeguimientoVuelo?> ObtenerSeguimiento(Guid usuarioId, Guid vueloId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.UsuarioId == usuarioId
                            && s.VueloId == vueloId
                            && s.FechaFin == null);
        }

        public async Task<List<SeguimientoVuelo>> BuscarPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Where(s =>
                    (s.FechaInicio >= fechaInicio && s.FechaInicio <= fechaFin) ||
                    (s.FechaFin != null && s.FechaFin >= fechaInicio && s.FechaFin <= fechaFin))
                .ToListAsync();
        }
    }
}
