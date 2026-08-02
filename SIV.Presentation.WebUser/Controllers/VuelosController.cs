using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.WebUser.Services;
using SIV.Presentation.WebUser.ViewModels;

namespace SIV.Presentation.WebUser.Controllers
{
    public class VuelosController : Controller
    {
        private readonly IPublicVueloService _publicVueloService;
        private readonly ISeguimientoService _seguimientoService;

        public VuelosController(IPublicVueloService publicVueloService, ISeguimientoService seguimientoService)
        {
            _publicVueloService = publicVueloService;
            _seguimientoService = seguimientoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? tipo,
            Guid? aerolineaId,
            Guid? aeropuertoId,
            DateTimeOffset? fecha,
            EstadoVuelo? estado,
            CancellationToken cancellationToken)
        {
            var catalogo = await _publicVueloService.ObtenerCatalogoAsync(cancellationToken);
            catalogo.AerolineaId = aerolineaId;
            catalogo.Fecha = fecha;
            catalogo.Estado = estado;

            var tipoSeleccionado = string.Equals(tipo, "salidas", StringComparison.OrdinalIgnoreCase)
                ? "salidas"
                : string.Equals(tipo, "todos", StringComparison.OrdinalIgnoreCase)
                    ? "todos"
                    : "llegadas";

            if (tipoSeleccionado == "llegadas")
            {
                catalogo.AeropuertoDestinoId = aeropuertoId;
            }
            else if (tipoSeleccionado == "salidas")
            {
                catalogo.AeropuertoOrigenId = aeropuertoId;
            }

            ViewBag.Tipo = tipoSeleccionado;
            ViewBag.AeropuertoId = aeropuertoId;

            catalogo.Vuelos = await _publicVueloService.ObtenerVuelosAsync(catalogo, cancellationToken);

            return View(catalogo);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(Guid id, CancellationToken cancellationToken)
        {
            var detalle = await _publicVueloService.ObtenerDetalleAsync(id, cancellationToken);
            if (detalle.Vuelo.Id == Guid.Empty)
            {
                return NotFound();
            }

            return View(detalle);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguir(Guid vueloId, CancellationToken cancellationToken)
        {
            await _seguimientoService.AgregarSeguimientoAsync(vueloId, cancellationToken);
            TempData["Success"] = "Vuelo agregado a tus seguimientos.";
            return RedirectToAction("Detalle", new { id = vueloId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DejarDeSeguir(Guid vueloId, CancellationToken cancellationToken)
        {
            await _seguimientoService.DejarSeguirAsync(vueloId, cancellationToken);
            TempData["Success"] = "Vuelo eliminado de tus seguimientos.";
            return RedirectToAction("Detalle", new { id = vueloId });
        }
    }
}
