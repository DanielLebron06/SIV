using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.FIDS.Services;
using SIV.Presentation.FIDS.ViewModels;

namespace SIV.Presentation.FIDS.Controllers
{
    public class TableroController : Controller
    {
        private readonly IFidsApiClient _apiClient;
        private readonly IConfiguration _configuration;

        public TableroController(IFidsApiClient apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(General));
        }

        [HttpGet]
        public async Task<IActionResult> Salidas(string? aeropuerto, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, TipoPantallaFids.Salidas, cancellationToken);
            modelo.Titulo = "SALIDAS";
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Llegadas(string? aeropuerto, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, TipoPantallaFids.Llegadas, cancellationToken);
            modelo.Titulo = "LLEGADAS";
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> General(string? aeropuerto, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, TipoPantallaFids.General, cancellationToken);
            modelo.Titulo = "TABLERO GENERAL";
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> TablaPartial(string? aeropuerto, TipoPantallaFids tipoPantalla, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, tipoPantalla, cancellationToken);
            return PartialView("_TablaFidsPartial", modelo);
        }

        private async Task<FidsViewModel> ObtenerTableroAsync(string? aeropuerto, TipoPantallaFids tipoPantalla, CancellationToken cancellationToken)
        {
            var ahora = DateTimeOffset.Now;
            var codigoAeropuerto = string.IsNullOrWhiteSpace(aeropuerto)
                ? (_configuration["ApiSettings:Aeropuerto"] ?? "SDQ").Trim().ToUpperInvariant()
                : aeropuerto.Trim().ToUpperInvariant();

            var respuesta = await _apiClient.GetVuelosAsync(codigoAeropuerto, tipoPantalla, TimeSpan.FromHours(12), cancellationToken);

            return new FidsViewModel
            {
                Aeropuerto = codigoAeropuerto,
                TipoPantalla = tipoPantalla,
                UltimaActualizacion = DateTime.Now,
                ApiDisponible = respuesta.Disponible,
                Vuelos = respuesta.Disponible
                    ? VueloMapper.Mapear(respuesta.Vuelos, tipoPantalla, codigoAeropuerto, ahora)
                    : new List<FilaVueloViewModel>()
            };
        }
    }
}
