using System;

namespace SIV.Presentation.Desktop.Notificaciones
{
    public class NotificacionDTO
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public Guid UsuarioId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
    }
}
