using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIV.Application.Domain.Entities
{
    public class HistorialNotificacion
    {
        [Key]
        public int IdNotificacion { get; set; }
        public int IdCambioOperativo { get; set; }

        [ForeignKey("IdCambioOperativo")]
        public CambioOperativo? CambioOperativo { get; set; }
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }


        [Required, MaxLength(20)]
        public string Medio { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Estado { get; set; } = string.Empty;

        [Required]
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    }
}