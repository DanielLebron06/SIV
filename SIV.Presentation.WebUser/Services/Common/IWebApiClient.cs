using SIV.Presentation.WebUser.ViewModels.Cuenta;
using SIV.Presentation.WebUser.ViewModels.Seguimiento;
using SIV.Presentation.WebUser.ViewModels.Vuelos;

namespace SIV.Presentation.WebUser.Services.Common
{
    public interface IWebApiClient
    {
        Task<List<VueloViewModel>> GetVuelosAsync(FiltrosVuelosViewModel filtros, CancellationToken cancellationToken = default);
        Task<VueloViewModel?> GetVueloAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<HistorialEstadoViewModel>> GetHistorialAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<AerolineaViewModel>> GetAerolineasAsync(CancellationToken cancellationToken = default);
        Task<List<AeropuertoViewModel>> GetAeropuertosAsync(CancellationToken cancellationToken = default);
        Task<string> LoginAsync(LoginViewModel login, CancellationToken cancellationToken = default);
        Task<string> RegistroAsync(RegistroViewModel registro, CancellationToken cancellationToken = default);
        Task<List<SeguimientoVueloViewModel>> GetSeguimientosAsync(CancellationToken cancellationToken = default);
        Task<List<NotificacionViewModel>> GetNotificacionesAsync(CancellationToken cancellationToken = default);
        Task MarcarNotificacionLeidaAsync(Guid id, CancellationToken cancellationToken = default);
        Task AgregarSeguimientoAsync(Guid vueloId, CancellationToken cancellationToken = default);
        Task DejarSeguirAsync(Guid vueloId, CancellationToken cancellationToken = default);
    }
}
