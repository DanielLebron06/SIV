namespace SIV.Application.DTOs.Reportes
{
    public class VueloMasSeguidoDTO
    {
        public Guid VueloId { get; set; }

        public string NumeroVuelo { get; set; } = string.Empty;

        public int TotalSeguidores { get; set; }
    }
}