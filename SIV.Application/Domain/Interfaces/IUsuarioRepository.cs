using SIV.Application.Common;
using SIV.Application.Domain.Entities;

namespace SIV.Application.Domain.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario> BuscarPorEmail(string email);
        Task<List<Usuario>> BuscarPorRol(Rol rol);
        Task<List<Usuario>> BuscarActivos();
    }
}
