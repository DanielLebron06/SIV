using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Usuario
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Rol Rol { get; set; }
    }
}
