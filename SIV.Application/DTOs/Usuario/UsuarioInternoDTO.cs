using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Usuario
{
    public class UsuarioInternoDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public Rol Rol { get; set; }

        public bool Activo { get; set; }
    }
}
