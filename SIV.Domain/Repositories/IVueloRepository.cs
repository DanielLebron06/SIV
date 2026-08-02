using SIV.Domain.Entities;
using SIV.Domain.Common;

namespace SIV.Domain.Repositories
{
    public interface IVueloRepository : IBaseRepository<Vuelo>
    {
        Task<Vuelo?> BuscarPorNumeroVuelo(string numeroVuelo);
        Task<bool> ExisteDuplicadoAsync(string numeroVuelo, Guid aerolineaId, Guid aeropuertoOrigenId, Guid aeropuertoDestinoId, DateTimeOffset fechaSalida);
        Task<List<Vuelo>> BuscarPorAerolinea(Guid aerolineaId);
        Task<List<Vuelo>> BuscarPorAeropuerto(Guid aeropuertoId);
        Task<bool> ExistenVuelosActivosPorAerolineaAsync(Guid aerolineaId);
        Task<bool> ExistenVuelosActivosPorAeropuertoAsync(Guid aeropuertoId);
        Task<List<Vuelo>> BuscarConFiltros(FiltrosVuelos filtros);
        Task<List<Vuelo>> BuscarPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<Vuelo?> GetVueloConDetallesAsync(Guid id);
    }
}
