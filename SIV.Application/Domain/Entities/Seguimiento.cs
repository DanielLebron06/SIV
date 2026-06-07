using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIV.Application.Domain.Entities
{
    public class Seguimiento
    {
        [Key]
        public int IdSeguimiento { get; set; }

        public int IdVuelo { get; set; }

        [ForeignKey("IdVuelo")]
        public Vuelo? Vuelo { get; set; }
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }
    }
}