using SIV.Presentation.WebUser.Services.Common;
using SIV.Presentation.WebUser.ViewModels.Seguimiento;

namespace SIV.Presentation.WebUser.Services.Seguimiento
{
    public interface ISeguimientoService
    {
        Task<List<SeguimientoVueloViewModel>> ObtenerSeguimientosAsync(CancellationToken cancellationToken = default);
        Task<List<NotificacionViewModel>> ObtenerNotificacionesAsync(CancellationToken cancellationToken = default);
        Task MarcarNotificacionLeidaAsync(Guid notificacionId, CancellationToken cancellationToken = default);
        Task AgregarSeguimientoAsync(Guid vueloId, CancellationToken cancellationToken = default);
        Task DejarSeguirAsync(Guid vueloId, CancellationToken cancellationToken = default);
    }

    public class SeguimientoService : ISeguimientoService
    {
        private readonly IWebApiClient _client;

        public SeguimientoService(IWebApiClient client)
        {
            _client = client;
        }

        public Task<List<SeguimientoVueloViewModel>> ObtenerSeguimientosAsync(CancellationToken cancellationToken = default)
        {
            return _client.GetSeguimientosAsync(cancellationToken);
        }

        public Task<List<NotificacionViewModel>> ObtenerNotificacionesAsync(CancellationToken cancellationToken = default)
        {
            return _client.GetNotificacionesAsync(cancellationToken);
        }

        public Task MarcarNotificacionLeidaAsync(Guid notificacionId, CancellationToken cancellationToken = default)
        {
            return _client.MarcarNotificacionLeidaAsync(notificacionId, cancellationToken);
        }

        public Task AgregarSeguimientoAsync(Guid vueloId, CancellationToken cancellationToken = default)
        {
            return _client.AgregarSeguimientoAsync(vueloId, cancellationToken);
        }

        public Task DejarSeguirAsync(Guid vueloId, CancellationToken cancellationToken = default)
        {
            return _client.DejarSeguirAsync(vueloId, cancellationToken);
        }
    }
}
