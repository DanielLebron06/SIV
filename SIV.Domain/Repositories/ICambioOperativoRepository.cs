using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface ICambioOperativoRepository : IBaseRepository<CambioOperativo>
    {
        Task<List<CambioOperativo>> BuscarPorVueloAsync(Guid vueloId);

        Task<List<CambioOperativo>> BuscarPorPeriodoAsync(
                    DateTime fechaInicio,
                    DateTime fechaFin);

    }

}
