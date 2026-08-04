using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SIV.Presentation.FIDS.Services.Dtos;

namespace SIV.Presentation.FIDS.Services
{
    public class FidsApiClient : IFidsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _aeropuertoDefault;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FidsApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _aeropuertoDefault = (configuration["ApiSettings:Aeropuerto"] ?? "SDQ").Trim().ToUpperInvariant();
        }

        public async Task<FidsRespuestaVuelos> GetVuelosAsync(string? aeropuerto, TipoPantallaFids tipoPantalla, TimeSpan? rangoHoras, CancellationToken cancellationToken = default)
        {
            var codigo = ResolverAeropuerto(aeropuerto);
            return await ObtenerConReintentoAsync(ConstruirQuery(codigo, tipoPantalla, rangoHoras), cancellationToken);
        }

        public string ResolverAeropuerto(string? aeropuerto)
        {
            return string.IsNullOrWhiteSpace(aeropuerto)
                ? _aeropuertoDefault
                : aeropuerto.Trim().ToUpperInvariant();
        }

        private async Task<FidsRespuestaVuelos> ObtenerConReintentoAsync(string query, CancellationToken cancellationToken)
        {
            const int intentosMaximos = 3;
            var delay = TimeSpan.FromSeconds(2);

            for (var intento = 1; intento <= intentosMaximos; intento++)
            {
                try
                {
                    var vuelos = await _httpClient.GetFromJsonAsync<List<VueloFidsDto>>("api/v1/fids/vuelos" + query, JsonOptions, cancellationToken);
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

        private static string ConstruirQuery(string aeropuerto, TipoPantallaFids tipoPantalla, TimeSpan? rangoHoras)
        {
            var query = "?tipoPantalla=" + tipoPantalla.ToString()
                      + "&aeropuerto=" + Uri.EscapeDataString(aeropuerto);

            if (rangoHoras.HasValue)
            {
                query += "&rangoHoras=" + Uri.EscapeDataString(rangoHoras.Value.ToString());
            }

            return query;
        }
    }
}
