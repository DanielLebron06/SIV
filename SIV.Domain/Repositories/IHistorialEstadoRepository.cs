using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface IHistorialEstadoRepository : IBaseRepository<HistorialEstado>
    {
        Task<List<HistorialEstado>> ObtenerPorVueloAsync(Guid vueloId);
    }
}
