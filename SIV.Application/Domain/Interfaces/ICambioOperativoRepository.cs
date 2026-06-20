using SIV.Application.Domain.Entities;

namespace SIV.Application.Domain.Interfaces
{
    public interface ICambioOperativoRepository : IBaseRepository<CambioOperativo>
    {
        Task<List<CambioOperativo>>BuscarPorVuelo(Guid vueloId);
    }

}
