using SIV.Domain.Entities;
using SIV.Domain.Common;

namespace SIV.Domain.Repositories
{
    public interface IVueloRepository : IBaseRepository<Vuelo>
    {
        Task<Vuelo?> BuscarPorNumeroVuelo(string numeroVuelo);
        Task<List<Vuelo>> BuscarPorAerolinea(Guid aerolineaId);
        Task<List<Vuelo>> BuscarPorAeropuerto(Guid aeropuertoId);
        Task<List<Vuelo>> BuscarConFiltros(FiltrosVuelos filtros);
    }
}
