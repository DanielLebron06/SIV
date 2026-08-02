using System;

namespace SIV.Domain.Entities
{
    public class Notificacion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VueloId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
        public bool Leida { get; set; } = false; 

        public Vuelo? Vuelo { get; set; }
        public Usuario? Usuario { get; set; }
    }
}