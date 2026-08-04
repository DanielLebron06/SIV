namespace SIV.Presentation.WebUser.ViewModels.Seguimiento
{
    public class SeguimientoVueloViewModel
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string AeropuertoOrigenIATA { get; set; } = string.Empty;
        public string AeropuertoDestinoIATA { get; set; } = string.Empty;
        public int EstadoActual { get; set; }
    }
}
