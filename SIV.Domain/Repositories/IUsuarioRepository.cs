using SIV.Domain.Emuns;
using SIV.Domain.Entities;

namespace SIV.Domain.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> BuscarPorEmail(string email);
        Task<List<Usuario>> BuscarPorRol(Rol rol);
        Task<List<Usuario>> BuscarActivos();
        Task<List<Usuario>> BuscarInternos();
    }
}
