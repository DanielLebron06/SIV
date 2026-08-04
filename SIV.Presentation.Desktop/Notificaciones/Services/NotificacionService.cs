using SIV.Presentation.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Notificaciones
{
    public class NotificacionService : INotificacionService
    {
        private readonly ApiClient _apiClient;

        public NotificacionService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<NotificacionDTO>> ObtenerNotificacionesAsync()
        {
            return await _apiClient.GetAsync<List<NotificacionDTO>>("Usuarios/notificaciones");
        }

        public async Task<List<NotificacionDTO>> ObtenerNotificacionesAdminAsync(Guid? vueloId, string numeroVuelo, Guid? usuarioId, string emailUsuario, DateTime? fechaInicio, DateTime? fechaFin, bool? leida)
        {
            var parametros = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("vueloId", vueloId),
                new KeyValuePair<string, object>("numeroVuelo", string.IsNullOrWhiteSpace(numeroVuelo) ? null : numeroVuelo),
                new KeyValuePair<string, object>("usuarioId", usuarioId),
                new KeyValuePair<string, object>("emailUsuario", string.IsNullOrWhiteSpace(emailUsuario) ? null : emailUsuario),
                new KeyValuePair<string, object>("fechaInicio", fechaInicio),
                new KeyValuePair<string, object>("fechaFin", fechaFin),
                new KeyValuePair<string, object>("leida", leida)
            };
            var query = QueryBuilder.Build(parametros);
            return await _apiClient.GetAsync<List<NotificacionDTO>>($"Notificaciones/admin{(string.IsNullOrEmpty(query) ? string.Empty : "?" + query)}");
        }

        public async Task MarcarNotificacionLeidaAsync(Guid id)
        {
            await _apiClient.PutAsync($"Notificaciones/{id}/leida");
        }
    }
}
