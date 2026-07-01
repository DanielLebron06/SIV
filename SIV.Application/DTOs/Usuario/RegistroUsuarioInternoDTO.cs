using SIV.Domain.Emuns;

namespace SIV.Application.DTOs.Usuario
{
    public class RegistroUsuarioInternoDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Rol Rol { get; set; }
    }
}