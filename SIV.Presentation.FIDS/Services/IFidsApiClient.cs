using SIV.Presentation.FIDS.Services.Dtos;

namespace SIV.Presentation.FIDS.Services
{
    public interface IFidsApiClient
    {
        Task<FidsRespuestaVuelos> GetVuelosAsync(string? aeropuerto, TipoPantallaFids tipoPantalla, TimeSpan? rangoHoras, CancellationToken cancellationToken = default);
    }

    public class FidsRespuestaVuelos
    {
        public bool Disponible { get; set; }
        public List<VueloFidsDto> Vuelos { get; set; } = new();
    }
}
