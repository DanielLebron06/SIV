using SIV.Application.Common;
using SIV.Application.Domain.Entities;
using SIV.Application.Domain.Interfaces;

public interface IVueloRepository : IBaseRepository<Vuelo>
{
    Task<Vuelo> BuscarPorCodigo(string codigo);
    Task<List<Vuelo>> BuscarPorAerolinea(string codigoAerolinea);
    Task<List<Vuelo>> BuscarPorAeropuerto(string codigoAeropuerto);
    Task<List<Vuelo>> BuscarConFiltros();
}