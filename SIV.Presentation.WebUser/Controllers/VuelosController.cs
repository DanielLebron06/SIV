using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.WebUser.Services.Common;
using SIV.Presentation.WebUser.Services.Seguimiento;
using SIV.Presentation.WebUser.Services.Vuelos;
using SIV.Presentation.WebUser.ViewModels.Vuelos;

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
            Guid? aerolineaId,
            Guid? aeropuertoOrigenId,
            Guid? aeropuertoDestinoId,
            DateTimeOffset? fecha,
            EstadoVuelo? estado,
            CancellationToken cancellationToken)
        {
            var catalogo = await _publicVueloService.ObtenerCatalogoAsync(cancellationToken);
            catalogo.AerolineaId = aerolineaId;
            catalogo.AeropuertoOrigenId = aeropuertoOrigenId;
            catalogo.AeropuertoDestinoId = aeropuertoDestinoId;
            catalogo.Fecha = fecha;
            catalogo.Estado = estado;

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

            if (User.Identity?.IsAuthenticated == true)
            {
                detalle.EstaSiguiendo = await EstaSiguiendoAsync(id, cancellationToken);
            }

            return View(detalle);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguir(Guid vueloId, CancellationToken cancellationToken)
        {
            try
            {
                await _seguimientoService.AgregarSeguimientoAsync(vueloId, cancellationToken);
                if (EsPeticionAjax())
                {
                    return Json(new { success = true, siguiendo = true, message = "Vuelo agregado a tus seguimientos." });
                }
                TempData["Success"] = "Vuelo agregado a tus seguimientos.";
                return RedirectToAction("Detalle", new { id = vueloId });
            }
            catch (ApiException ex)
            {
                if (EsPeticionAjax())
                {
                    return Json(new { success = false, siguiendo = false, message = ex.Message });
                }
                TempData["Error"] = MensajesError.ObtenerMensaje(ex);
                return RedirectToAction("Detalle", new { id = vueloId });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DejarDeSeguir(Guid vueloId, CancellationToken cancellationToken)
        {
            try
            {
                await _seguimientoService.DejarSeguirAsync(vueloId, cancellationToken);
                if (EsPeticionAjax())
                {
                    return Json(new { success = true, siguiendo = false, message = "Vuelo eliminado de tus seguimientos." });
                }
                TempData["Success"] = "Vuelo eliminado de tus seguimientos.";
                return RedirectToAction("Detalle", new { id = vueloId });
            }
            catch (ApiException ex)
            {
                if (EsPeticionAjax())
                {
                    return Json(new { success = false, siguiendo = false, message = ex.Message });
                }
                TempData["Error"] = MensajesError.ObtenerMensaje(ex);
                return RedirectToAction("Detalle", new { id = vueloId });
            }
        }

        private async Task<bool> EstaSiguiendoAsync(Guid vueloId, CancellationToken cancellationToken)
        {
            try
            {
                var seguimientos = await _seguimientoService.ObtenerSeguimientosAsync(cancellationToken);
                return seguimientos.Any(s => s.VueloId == vueloId);
            }
            catch (ApiException)
            {
                return false;
            }
        }

        private bool EsPeticionAjax()
        {
            return string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
        }
    }
}
