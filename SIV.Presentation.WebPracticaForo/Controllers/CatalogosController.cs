using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Aerolinea;
using SIV.Application.DTOs.Aeropuerto;
using SIV.Application.Features.Vuelos.Commands.DesactivarAerolinea;
using SIV.Application.Features.Vuelos.Commands.DesactivarAeropuerto;
using SIV.Application.Features.Vuelos.Commands.RegistrarAerolinea;
using SIV.Application.Features.Vuelos.Commands.RegistrarAeropuerto;
using SIV.Application.Features.Vuelos.Queries.ObtenerAerolineas;
using SIV.Application.Features.Vuelos.Queries.ObtenerAeropuertos;
using System.Security.Claims;

namespace SIV.Presentation.Web.Controllers
{
    public class CatalogosController : Controller
    {
        private readonly ISender _sender;
        private static readonly Guid UsuarioPruebasId = Guid.Parse("352717f7-14e6-4dfe-a183-aeaa21717ae3");

        public CatalogosController(ISender sender)
        {
            _sender = sender;
        }

        private Guid ObtenerUsuarioActual()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return UsuarioPruebasId;

            return Guid.Parse(userId);
        }

        // GET: Catalogos
        public IActionResult Index()
        {
            return View();
        }

        // GET: Catalogos/Aerolineas
        public async Task<IActionResult> Aerolineas()
        {
            var result = await _sender.Send(new ObtenerAerolineasQuery());

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(Enumerable.Empty<AerolineaDTO>());
            }

            return View(result.Data);
        }

        // POST: Catalogos/RegistrarAerolinea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarAerolinea(RegistroAerolineaDTO datos)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Aerolineas));
            }

            var result = await _sender.Send(new RegistrarAerolineaCommand
            {
                Datos = datos,
                EjecutadorId = ObtenerUsuarioActual()
            });

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Aerolínea registrada exitosamente.";
            }

            return RedirectToAction(nameof(Aerolineas));
        }

        // POST: Catalogos/DesactivarAerolinea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesactivarAerolinea(Guid id)
        {
            var result = await _sender.Send(new DesactivarAerolineaCommand
            {
                AerolineaId = id,
                EjecutadorId = ObtenerUsuarioActual()
            });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] = "Aerolínea desactivada correctamente.";

            return RedirectToAction(nameof(Aerolineas));
        }

        // GET: Catalogos/Aeropuertos
        public async Task<IActionResult> Aeropuertos()
        {
            var result = await _sender.Send(new ObtenerAeropuertosQuery());

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(Enumerable.Empty<AeropuertoDTO>());
            }

            return View(result.Data);
        }

        // POST: Catalogos/RegistrarAeropuerto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarAeropuerto(RegistroAeropuertoDTO datos)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Aeropuertos));

            var result = await _sender.Send(new RegistrarAeropuertoCommand
            {
                Datos = datos,
                EjecutadorId = ObtenerUsuarioActual()
            });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] = "Aeropuerto registrado exitosamente.";

            return RedirectToAction(nameof(Aeropuertos));
        }

        // POST: Catalogos/DesactivarAeropuerto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesactivarAeropuerto(Guid id)
        {
            var result = await _sender.Send(new DesactivarAeropuertoCommand
            {
                AeropuertoId = id,
                EjecutadorId = ObtenerUsuarioActual()
            });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] = "Aeropuerto desactivado correctamente.";

            return RedirectToAction(nameof(Aeropuertos));
        }
    }
}
