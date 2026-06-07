using System.ComponentModel.DataAnnotations;

namespace SIV.Application.Domain.Entities
{
    public class Catalogo
    {
        [Key]
        public int IdCatalogo { get; set; }

        [Required, MaxLength(20)]
        public string Tipo { get; set; } = string.Empty; // "Aerolinea" o "Aeropuerto"

        [Required, MaxLength(10)]
        public string Codigo { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
    }
}