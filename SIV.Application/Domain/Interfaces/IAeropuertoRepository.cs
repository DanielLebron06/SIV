
using SIV.Application.Domain.Entities;

namespace SIV.Application.Domain.Interfaces
{
    public interface IAeropuertoRepository : IBaseRepository<Aeropuerto>
    {
        Task<Aeropuerto> BuscarPorCodigo(string codigoAeropuerto);
        Task<List<Aeropuerto>> MostrarActivos();
    }
}
