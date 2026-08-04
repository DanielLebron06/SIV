namespace SIV.Presentation.WebApi.DTOs.Vuelos
{
    public class CambioOperativoTiempoDTO
    {
        public DateTimeOffset NuevaHoraEstimada { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
