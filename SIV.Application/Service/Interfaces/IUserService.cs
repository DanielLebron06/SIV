using SIV.Application.DTOs;
using SIV.Domain.Entities;
using SIV.Domain.Emuns;

namespace SIV.Application.Service.Interfaces
{
    public interface IUserService
    {
        Task RegistraUsuarioPublico(UsuarioDTO usuario);
        Task RegistraUsuarioInterno(UsuarioDTO usuario, Usuario ejecutador);
        Task<UsuarioDTO> InicioSesion(UsuarioDTO usuario);
        Task DesactivarUsuario(Guid idUsuario, Usuario ejecutador);
        Task SeguirVuelo(Guid VueloId, UsuarioDTO usuario);
        Task DejarSeguirVuelo(Guid VueloId, UsuarioDTO usuario);
        Task<List<SeguimientoVueloDTO>> ObtenerSeguidosDeUsuario(UsuarioDTO usuario);
        Task<List<NotificacionDTO>> ObtnerNotificaciones(UsuarioDTO usuario);
    }
}
