
using SIV.Domain.Entities;
using SIV.Application.DTOs.Usuario;
using SIV.Application.DTOs.Seguimiento;
using SIV.Application.DTOs.Notificacion;

namespace SIV.Application.Service.Interfaces
{
    public interface IUserService
    {
        Task RegistraUsuarioPublico(RegistroUsuarioDTO nuevoUsuario);
        Task RegistraUsuarioInterno(RegistroUsuarioInternoDTO nuevoUsuario, Usuario ejecutador);
        Task<UsuarioDTO> InicioSesion(LoginDTO usuario);
        Task DesactivarUsuario(Guid idUsuario, Usuario ejecutador);
        Task SeguirVuelo(Guid vueloId, Usuario usuariousuarioAutenticado);
        Task DejarSeguirVuelo(Guid vueloId, Usuario usuarioAutenticado);
        Task<List<SeguimientoVueloDTO>> ObtenerSeguidosDeUsuario(Usuario usuarioAutenticado);
        Task<List<NotificacionDTO>> ObtnerNotificaciones(Usuario usuarioAutenticado);
    }
}
