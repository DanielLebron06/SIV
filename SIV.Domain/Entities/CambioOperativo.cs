using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIV.Domain.Entities
{
    public class CambioOperativo
    {
        [Key]
        public int IdCambio { get; set; }

        public int IdVuelo { get; set; }

        [ForeignKey("IdVuelo")]
        public Vuelo? Vuelo { get; set; }



        public int IdUsuarioResponsable { get; set; }

        [ForeignKey("IdUsuarioResponsable")]
        public Usuario? UsuarioResponsable { get; set; }


        [Required, MaxLength(30)]
        public string EstadoAnterior { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string NuevoEstado { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Justificacion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}