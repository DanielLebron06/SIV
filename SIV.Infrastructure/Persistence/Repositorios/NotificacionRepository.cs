using Microsoft.EntityFrameworkCore;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class NotificacionRepository
    : BaseRepository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(SIVDbContext context) : base(context)
        {
        }

        public async Task<List<Notificacion>> BuscarPorUsuarioAsync(Guid usuarioId)
        {
            return await _dbSet
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }

        public async Task<List<Notificacion>> BuscarConFiltrosAsync(FiltrosNotificaciones filtros)
        {
            var query = _dbSet.AsNoTracking()
                .Include(n => n.Vuelo)
                .Include(n => n.Usuario)
                .AsQueryable();

            if (filtros.VueloId.HasValue)
                query = query.Where(n => n.VueloId == filtros.VueloId);

            if (!string.IsNullOrWhiteSpace(filtros.NumeroVuelo))
            {
                var numeroVueloLimpio = filtros.NumeroVuelo.Trim().ToLower();
                query = query.Where(n => n.Vuelo != null && n.Vuelo.NumeroVuelo.ToLower() == numeroVueloLimpio);
            }

            if (filtros.UsuarioId.HasValue)
                query = query.Where(n => n.UsuarioId == filtros.UsuarioId);

            if (!string.IsNullOrWhiteSpace(filtros.EmailUsuario))
            {
                var emailLimpio = filtros.EmailUsuario.Trim().ToLower();
                query = query.Where(n => n.Usuario != null && n.Usuario.Email.ToLower() == emailLimpio);
            }

            if (filtros.FechaInicio.HasValue)
                query = query.Where(n => n.FechaEnvio >= filtros.FechaInicio);

            if (filtros.FechaFin.HasValue)
                query = query.Where(n => n.FechaEnvio <= filtros.FechaFin);

            if (filtros.Leida.HasValue)
                query = query.Where(n => n.Leida == filtros.Leida);

            return await query
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }

        public async Task<int> ContarNoLeidasAsync(Guid usuarioId)
        {
            return await _dbSet
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .CountAsync();
        }

        public async Task MarcarTodasComoLeidasAsync(Guid usuarioId)
        {
            var notificaciones = await _dbSet
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .ToListAsync();

            foreach (var notificacion in notificaciones)
            {
                notificacion.Leida = true;
            }
        }
    }
}
