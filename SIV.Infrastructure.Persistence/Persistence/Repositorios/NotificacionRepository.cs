using Microsoft.EntityFrameworkCore;
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
