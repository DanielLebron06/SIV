using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Usuarios
{
    public interface IUserService
    {
        Task RegistrarUsuarioPublicoAsync(RegistroUsuarioDTO usuario);
        Task RegistrarUsuarioInternoAsync(RegistroUsuarioInternoDTO usuario);
        Task<List<UsuarioInternoDTO>> ObtenerUsuariosInternosAsync();
        Task DesactivarUsuarioAsync(Guid id);
        Task<List<SeguimientoVueloDTO>> ObtenerSeguimientosAsync();
        Task SeguirVueloAsync(Guid vueloId);
        Task DejarSeguirVueloAsync(Guid vueloId);
    }
}
