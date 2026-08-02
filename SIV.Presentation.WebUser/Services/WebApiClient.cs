using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using SIV.Presentation.WebUser.Exceptions;
using SIV.Presentation.WebUser.ViewModels;

namespace SIV.Presentation.WebUser.Services
{
    public class WebApiClient : IWebApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WebApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<VueloViewModel>> GetVuelosAsync(FiltrosVuelosViewModel filtros, CancellationToken cancellationToken = default)
        {
            var query = ConstruirQuery(filtros);
            var vuelos = await GetAsync<List<VueloViewModel>>($"api/v1/vuelos{query}", cancellationToken);
            return vuelos ?? new List<VueloViewModel>();
        }

        public Task<VueloViewModel?> GetVueloAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return GetAsync<VueloViewModel>($"api/v1/vuelos/{id}", cancellationToken);
        }

        public async Task<List<HistorialEstadoViewModel>> GetHistorialAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var resultado = await GetAsync<List<HistorialEstadoViewModel>>($"api/v1/vuelos/{id}/historial", cancellationToken);
            return resultado ?? new List<HistorialEstadoViewModel>();
        }

        public async Task<List<AerolineaViewModel>> GetAerolineasAsync(CancellationToken cancellationToken = default)
        {
            var resultado = await GetAsync<List<AerolineaViewModel>>("api/v1/catalogos/aerolineas", cancellationToken);
            return resultado ?? new List<AerolineaViewModel>();
        }

        public async Task<List<AeropuertoViewModel>> GetAeropuertosAsync(CancellationToken cancellationToken = default)
        {
            var resultado = await GetAsync<List<AeropuertoViewModel>>("api/v1/catalogos/aeropuertos", cancellationToken);
            return resultado ?? new List<AeropuertoViewModel>();
        }

        public async Task<string> LoginAsync(LoginViewModel login, CancellationToken cancellationToken = default)
        {
            var respuesta = await PostAsync<LoginViewModel, LoginResponse>(
                "api/v1/auth/login",
                new LoginViewModel { Email = login.Email, Password = login.Password },
                cancellationToken);
            return respuesta.Token;
        }

        public async Task<string> RegistroAsync(RegistroViewModel registro, CancellationToken cancellationToken = default)
        {
            var respuesta = await PostAsync<RegistroViewModel, MensajeResponse>(
                "api/v1/usuarios/registro-publico",
                new RegistroViewModel { Email = registro.Email, Password = registro.Password },
                cancellationToken);
            return respuesta.Mensaje ?? string.Empty;
        }

        public async Task<List<SeguimientoVueloViewModel>> GetSeguimientosAsync(CancellationToken cancellationToken = default)
        {
            var resultado = await GetAsync<List<SeguimientoVueloViewModel>>("api/v1/usuarios/seguimientos", cancellationToken);
            return resultado ?? new List<SeguimientoVueloViewModel>();
        }

        public async Task<List<NotificacionViewModel>> GetNotificacionesAsync(CancellationToken cancellationToken = default)
        {
            var resultado = await GetAsync<List<NotificacionViewModel>>("api/v1/usuarios/notificaciones", cancellationToken);
            return resultado ?? new List<NotificacionViewModel>();
        }

        public async Task AgregarSeguimientoAsync(Guid vueloId, CancellationToken cancellationToken = default)
        {
            var cuerpo = new AgregarSeguimientoRequest { VueloId = vueloId };
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/usuarios/seguimiento")
            {
                Content = JsonContent.Create(cuerpo, options: JsonOptions)
            };
            await EnviarAsync(request, cancellationToken);
        }

        public async Task DejarSeguirAsync(Guid vueloId, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/usuarios/seguimiento/{vueloId}");
            await EnviarAsync(request, cancellationToken);
        }

        private async Task<T?> GetAsync<T>(string ruta, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ruta);
            var response = await EnviarAsync(request, cancellationToken);
            return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
        }

        private async Task<TRespuesta> PostAsync<TCuerpo, TRespuesta>(string ruta, TCuerpo cuerpo, CancellationToken cancellationToken)
            where TRespuesta : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ruta)
            {
                Content = JsonContent.Create(cuerpo, options: JsonOptions)
            };
            var response = await EnviarAsync(request, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TRespuesta>(JsonOptions, cancellationToken) ?? new TRespuesta();
        }

        private async Task<HttpResponseMessage> EnviarAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AgregarToken(request);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            throw await CrearExcepcionAsync(response, cancellationToken);
        }

        private void AgregarToken(HttpRequestMessage request)
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirstValue("Token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private static async Task<ApiException> CrearExcepcionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var statusCode = (int)response.StatusCode;
            try
            {
                var cuerpo = await response.Content.ReadFromJsonAsync<MensajeResponse>(JsonOptions, cancellationToken);
                if (!string.IsNullOrWhiteSpace(cuerpo?.Mensaje))
                {
                    return new ApiException(statusCode, cuerpo.Mensaje);
                }
            }
            catch
            {
            }

            return new ApiException(statusCode, "Ocurrió un error al comunicarse con el servidor.");
        }

        private static string ConstruirQuery(FiltrosVuelosViewModel filtros)
        {
            var parametros = new List<string>();
            if (filtros.AerolineaId.HasValue)
            {
                parametros.Add($"aerolineaId={filtros.AerolineaId.Value}");
            }
            if (filtros.AeropuertoOrigenId.HasValue)
            {
                parametros.Add($"aeropuertoOrigenId={filtros.AeropuertoOrigenId.Value}");
            }
            if (filtros.AeropuertoDestinoId.HasValue)
            {
                parametros.Add($"aeropuertoDestinoId={filtros.AeropuertoDestinoId.Value}");
            }
            if (filtros.Fecha.HasValue)
            {
                parametros.Add($"fecha={Uri.EscapeDataString(filtros.Fecha.Value.ToString("yyyy-MM-dd"))}");
            }
            if (filtros.Estado.HasValue)
            {
                parametros.Add($"estado={(int)filtros.Estado.Value}");
            }
            return parametros.Count == 0 ? string.Empty : "?" + string.Join("&", parametros);
        }

        private sealed class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }

        private sealed class MensajeResponse
        {
            public string? Mensaje { get; set; }
        }

        private sealed class AgregarSeguimientoRequest
        {
            public Guid VueloId { get; set; }
        }
    }
}
