using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.FIDS.Services;
using SIV.Presentation.FIDS.ViewModels;

namespace SIV.Presentation.FIDS.Controllers
{
    public class TableroController : Controller
    {
        private readonly IFidsApiClient _apiClient;

        public TableroController(IFidsApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Salidas(string? aeropuerto, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, esSalida: true, cancellationToken);
            modelo.Titulo = "SALIDAS";
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Llegadas(string? aeropuerto, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, esSalida: false, cancellationToken);
            modelo.Titulo = "LLEGADAS";
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> TablaPartial(string? aeropuerto, bool esSalida, CancellationToken cancellationToken)
        {
            var modelo = await ObtenerTableroAsync(aeropuerto, esSalida, cancellationToken);
            return PartialView("_TablaFidsPartial", modelo);
        }

        private async Task<FidsViewModel> ObtenerTableroAsync(string? aeropuerto, bool esSalida, CancellationToken cancellationToken)
        {
            var ahora = DateTimeOffset.Now;
            var respuesta = await _apiClient.GetVuelosAsync(aeropuerto, esSalida, ahora, cancellationToken);

            return new FidsViewModel
            {
                Aeropuerto = aeropuerto,
                UltimaActualizacion = DateTime.Now,
                ApiDisponible = respuesta.Disponible,
                Vuelos = respuesta.Disponible
                    ? VueloMapper.Mapear(respuesta.Vuelos, esSalida, ahora)
                    : new List<FilaVueloViewModel>()
            };
        }
    }
}
