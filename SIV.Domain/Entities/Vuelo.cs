using System.ComponentModel.DataAnnotations;

namespace SIV.Domain.Entities
{
    public class Vuelo
    {
        [Key]
        public int IdVuelo { get; set; }

        [Required, MaxLength(10)]
        public string NumeroVuelo { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Aerolinea { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string AeropuertoOrigen { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string AeropuertoDestino { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string EstadoActual { get; set; } = "Programado";

        [Required]
        public DateTime SalidaProgramada { get; set; }

        [Required]
        public DateTime LlegadaProgramada { get; set; }

        [MaxLength(20)]
        public string? Gate { get; set; }
    }
}