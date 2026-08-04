namespace SIV.Presentation.WebApi.DTOs
{
    public class CambioOperativoTiempoDTO
    {
        public DateTimeOffset NuevaHoraEstimada { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
