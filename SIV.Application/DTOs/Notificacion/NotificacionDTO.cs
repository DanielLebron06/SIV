

namespace SIV.Application.DTOs.Notificacion
{
    public class NotificacionDTO
    {
        public Guid Id { get; set; }

        public Guid VueloId { get; set; }

        public string Titulo { get; set; }

        public string Mensaje { get; set; }

        public DateTime FechaEnvio { get; set; }

        public bool Leida { get; set; }

    }
}
