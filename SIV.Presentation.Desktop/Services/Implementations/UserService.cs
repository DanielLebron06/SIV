using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApiClient _apiClient;

        public UserService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task RegistrarUsuarioPublicoAsync(RegistroUsuarioDTO usuario)
        {
            await _apiClient.PostAsync("Usuarios/registro-publico", usuario);
        }

        public async Task RegistrarUsuarioInternoAsync(RegistroUsuarioInternoDTO usuario)
        {
            await _apiClient.PostAsync("Usuarios/registro-interno", usuario);
        }

        public async Task<List<UsuarioInternoDTO>> ObtenerUsuariosInternosAsync()
        {
            return await _apiClient.GetAsync<List<UsuarioInternoDTO>>("Usuarios/internos");
        }

        public async Task DesactivarUsuarioAsync(Guid id)
        {
            await _apiClient.PutAsync($"Usuarios/{id}/desactivar");
        }

        public async Task<List<NotificacionDTO>> ObtenerNotificacionesAsync()
        {
            return await _apiClient.GetAsync<List<NotificacionDTO>>("Usuarios/notificaciones");
        }

        public async Task<List<SeguimientoVueloDTO>> ObtenerSeguimientosAsync()
        {
            return await _apiClient.GetAsync<List<SeguimientoVueloDTO>>("Usuarios/seguimientos");
        }

        public async Task SeguirVueloAsync(Guid vueloId)
        {
            await _apiClient.PostAsync("Usuarios/seguimiento", new AgregarSeguimientoRequest { VueloId = vueloId });
        }

        public async Task DejarSeguirVueloAsync(Guid vueloId)
        {
            await _apiClient.DeleteAsync($"Usuarios/seguimiento/{vueloId}");
        }
    }
}
