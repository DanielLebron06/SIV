
using Microsoft.EntityFrameworkCore;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Infrastructure.Persistence.Repositorios
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SIVDbContext context) : base(context) { }

        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && u.Activo);
        }
        public async Task<List<Usuario>> BuscarPorRol(Rol rol)
        {
            return await _dbSet
                .Where(u => u.Rol == rol && u.Activo)
                .ToListAsync();
        }
        public async Task<List<Usuario>> BuscarActivos()
        {
            return await _dbSet
                .Where(u => u.Activo)
                .ToListAsync();
        }
    }
}
