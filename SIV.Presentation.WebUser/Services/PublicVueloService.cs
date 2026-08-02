using SIV.Presentation.WebUser.ViewModels;

namespace SIV.Presentation.WebUser.Services
{
    public interface IPublicVueloService
    {
        Task<FiltrosVuelosViewModel> ObtenerCatalogoAsync(CancellationToken cancellationToken = default);
        Task<List<VueloViewModel>> ObtenerVuelosAsync(FiltrosVuelosViewModel filtros, CancellationToken cancellationToken = default);
        Task<VueloDetalleViewModel> ObtenerDetalleAsync(Guid id, CancellationToken cancellationToken = default);
    }

    public class PublicVueloService : IPublicVueloService
    {
        private readonly IWebApiClient _client;

        public PublicVueloService(IWebApiClient client)
        {
            _client = client;
        }

        public async Task<FiltrosVuelosViewModel> ObtenerCatalogoAsync(CancellationToken cancellationToken = default)
        {
            var aerolineas = await _client.GetAerolineasAsync(cancellationToken);
            var aeropuertos = await _client.GetAeropuertosAsync(cancellationToken);
            return new FiltrosVuelosViewModel
            {
                Aerolineas = aerolineas.Where(a => a.Activa).ToList(),
                Aeropuertos = aeropuertos.Where(a => a.Activo).ToList()
            };
        }

        public Task<List<VueloViewModel>> ObtenerVuelosAsync(FiltrosVuelosViewModel filtros, CancellationToken cancellationToken = default)
        {
            return _client.GetVuelosAsync(filtros, cancellationToken);
        }

        public async Task<VueloDetalleViewModel> ObtenerDetalleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var vuelo = await _client.GetVueloAsync(id, cancellationToken);
            if (vuelo == null)
            {
                return new VueloDetalleViewModel();
            }

            var historial = await _client.GetHistorialAsync(id, cancellationToken);
            return new VueloDetalleViewModel
            {
                Vuelo = vuelo,
                Historial = historial
            };
        }
    }
}
