using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIV.Application.DTOs.Vuelo;
using SIV.Application.Features.Vuelos.Commands.ActualizarVuelo;
using SIV.Application.Features.Vuelos.Commands.CambiarEstadoVuelo;
using SIV.Application.Features.Vuelos.Commands.RegistrarVuelo;
using SIV.Application.Features.Vuelos.Queries.ConsultarVuelos;
using SIV.Application.Features.Vuelos.Queries.ObtenerAerolineas;
using SIV.Application.Features.Vuelos.Queries.ObtenerAeropuertos;
using SIV.Application.Features.Vuelos.Queries.ObtenerEstadosVuelo;
using SIV.Application.Features.Vuelos.Queries.ObtenerVuelo;
using SIV.Domain.Common;
using System.Security.Claims;

namespace SIV.Presentation.Web.Controllers
{
    public class VuelosController : Controller
    {
        private readonly ISender _sender;
        private static readonly Guid EjecutadorIdPruebas = Guid.Parse("bc09e684-9365-4a1b-9255-6373f3d29f1c");

        public VuelosController(ISender sender)
        {
            _sender = sender;
        }

        private Guid ObtenerEjecutadorId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? EjecutadorIdPruebas : Guid.Parse(userId);
        }

        private async Task CargarListasDesplegablesAsync()
        {
            var aerolineas = await _sender.Send(new ObtenerAerolineasQuery());
            var aeropuertos = await _sender.Send(new ObtenerAeropuertosQuery());

            ViewBag.Aerolineas = new SelectList(aerolineas.Data, "Id", "Nombre");
            ViewBag.Aeropuertos = new SelectList(aeropuertos.Data.Select(a => new
            {
                a.Id,
                Nombre = $"{a.Nombre} ({a.CodigoIATA})"
            }), "Id", "Nombre");
        }

        public async Task<IActionResult> Index(FiltrosVuelos filtros)
        {
            filtros ??= new FiltrosVuelos();
            var result = await _sender.Send(new ConsultarVuelosQuery { Filtros = filtros });

            return View(result.Success ? result.Data : Enumerable.Empty<DatosVueloDTO>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var vuelo = await _sender.Send(new ObtenerVueloQuery { VueloId = id });

            if (!vuelo.Success)
            {
                TempData["Error"] = vuelo.Message;
                return RedirectToAction(nameof(Index));
            }

            var historial = await _sender.Send(new ObtenerEstadosVueloQuery { VueloId = id });
            ViewBag.Historial = historial.Data;

            return View(vuelo.Data);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasDesplegablesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatosVueloDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasDesplegablesAsync();
                return View(dto);
            }

            var result = await _sender.Send(new RegistrarVueloCommand
            {
                Datos = dto,
                EjecutadorId = ObtenerEjecutadorId()
            });

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _sender.Send(new ObtenerVueloQuery { VueloId = id });
            await CargarListasDesplegablesAsync();

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DatosVueloDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasDesplegablesAsync();
                return View(dto);
            }

            await _sender.Send(new ActualizarVueloCommand
            {
                VueloId = id,
                Datos = dto,
                EjecutadorId = ObtenerEjecutadorId()
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
