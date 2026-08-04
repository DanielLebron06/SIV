namespace SIV.Presentation.WebUser.ViewModels.Seguimiento
{
    public class NotificacionViewModel
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
    }
}
