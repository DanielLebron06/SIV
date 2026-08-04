namespace SIV.Domain.Common
{
    public class FiltrosNotificaciones
    {
        public Guid? VueloId { get; set; }
        public string? NumeroVuelo { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? EmailUsuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Leida { get; set; }
    }
}
