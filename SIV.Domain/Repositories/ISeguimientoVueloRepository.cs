using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface ISeguimientoVueloRepository : IBaseRepository<SeguimientoVuelo>
    {
        Task<List<SeguimientoVuelo>> BuscarPorUsuario(Guid usuarioId);

        Task<List<SeguimientoVuelo>> BuscarActivosPorUsuario(Guid usuarioId);

        Task<List<SeguimientoVuelo>> BuscarInactivosPorUsuario(Guid usuarioId);

        Task<bool> ExisteSeguimiento(Guid usuarioId, Guid vueloId);
    }
}
