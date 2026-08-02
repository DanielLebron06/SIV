using System.Net.Http.Json;
using System.Text.Json;
using SIV.Presentation.FIDS.Services.Dtos;

namespace SIV.Presentation.FIDS.Services
{
    public class FidsApiClient : IFidsApiClient
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FidsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FidsRespuestaVuelos> GetVuelosAsync(string? aeropuerto, bool esSalida, DateTimeOffset? fecha, CancellationToken cancellationToken = default)
        {
            var respuesta = await ObtenerConReintentoAsync(ConstruirQuery(fecha), cancellationToken);
            if (!respuesta.Disponible)
            {
                return respuesta;
            }

            if (respuesta.Vuelos.Count == 0 && fecha.HasValue)
            {
                var sinFecha = await ObtenerConReintentoAsync(string.Empty, cancellationToken);
                if (sinFecha.Disponible && sinFecha.Vuelos.Count > 0)
                {
                    respuesta = sinFecha;
                }
            }

            if (!string.IsNullOrWhiteSpace(aeropuerto))
            {
                var iata = aeropuerto.Trim().ToUpperInvariant();
                respuesta.Vuelos = esSalida
                    ? respuesta.Vuelos.Where(v => string.Equals(v.AeropuertoOrigenIATA, iata, StringComparison.OrdinalIgnoreCase)).ToList()
                    : respuesta.Vuelos.Where(v => string.Equals(v.AeropuertoDestinoIATA, iata, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return respuesta;
        }

        private async Task<FidsRespuestaVuelos> ObtenerConReintentoAsync(string query, CancellationToken cancellationToken)
        {
            const int intentosMaximos = 3;
            var delay = TimeSpan.FromSeconds(2);

            for (var intento = 1; intento <= intentosMaximos; intento++)
            {
                try
                {
                    var vuelos = await _httpClient.GetFromJsonAsync<List<VueloFidsDto>>("api/v1/vuelos" + query, JsonOptions, cancellationToken);
                    return new FidsRespuestaVuelos { Disponible = true, Vuelos = vuelos ?? new List<VueloFidsDto>() };
                }
                catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
                {
                    if (intento == intentosMaximos)
                    {
                        return new FidsRespuestaVuelos { Disponible = false };
                    }
                    await Task.Delay(delay, cancellationToken);
                }
            }

            return new FidsRespuestaVuelos { Disponible = false };
        }

        private static string ConstruirQuery(DateTimeOffset? fecha)
        {
            if (!fecha.HasValue)
            {
                return string.Empty;
            }
            return "?fecha=" + Uri.EscapeDataString(fecha.Value.ToString("yyyy-MM-dd"));
        }
    }
}
