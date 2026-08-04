namespace SIV.Application.DTOs.Notificacion
{
    public class NotificacionAdminDTO
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public Guid UsuarioId { get; set; }
        public string? NumeroVuelo { get; set; }
        public string? EmailUsuario { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
    }
}
