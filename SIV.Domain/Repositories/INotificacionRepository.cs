using SIV.Domain.Common;
using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<List<Notificacion>> BuscarPorUsuarioAsync(Guid usuarioId);
        Task<List<Notificacion>> BuscarConFiltrosAsync(FiltrosNotificaciones filtros);
        Task<int> ContarNoLeidasAsync(Guid usuarioId);
        Task MarcarTodasComoLeidasAsync(Guid usuarioId);
    }

}
