using System.ComponentModel.DataAnnotations;

namespace SIV.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required, MaxLength(100)]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Rol { get; set; } = string.Empty;
    }
}