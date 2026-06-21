
using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface IAeropuertoRepository : IBaseRepository<Aeropuerto>
    {
        Task<Aeropuerto?> BuscarPorCodigoAsync(string codigoAeropuerto);
        Task<List<Aeropuerto>> ObtenerActivosAsync();
    }
}
