using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface IAerolineaRepository : IBaseRepository<Aerolinea>
    {
        Task<Aerolinea?> BuscarPorCodigoAsync(string codigoAerolinea);
        Task<bool> ExistePorCodigoAsync(string codigoAerolinea);
        Task<List<Aerolinea>> ObtenerActivosAsync();
    }
}
