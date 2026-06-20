

using SIV.Application.Domain.Entities;

namespace SIV.Application.Domain.Interfaces
{
    public interface IAerolíneaRepository: IBaseRepository<Aerolinea>
    {
        Task<Aerolinea> BuscarPorCodigo(string codigoAerolinea);
        Task<List<Aerolinea>> MostrarActivos();
    }
}
