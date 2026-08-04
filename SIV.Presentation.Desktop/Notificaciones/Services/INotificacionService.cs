using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Notificaciones
{
    public interface INotificacionService
    {
        Task<List<NotificacionDTO>> ObtenerNotificacionesAsync();
        Task<List<NotificacionDTO>> ObtenerNotificacionesAdminAsync(Guid? vueloId, string numeroVuelo, Guid? usuarioId, string emailUsuario, DateTime? fechaInicio, DateTime? fechaFin, bool? leida);
        Task MarcarNotificacionLeidaAsync(Guid id);
    }
}
