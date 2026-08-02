using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Features.Vuelos.Commands.CambiarEstadoVuelo;
using SIV.Domain.Emuns;
using System.Security.Claims;

namespace SIV.Presentation.Web.Controllers
{
    public class VueloEstadosController : Controller
    {
        private readonly ISender _sender;
        private static readonly Guid EjecutadorIdPruebas = Guid.Parse("bc09e684-9365-4a1b-9255-6373f3d29f1c");

        public VueloEstadosController(ISender sender) => _sender = sender;

        private Guid ObtenerEjecutadorId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? EjecutadorIdPruebas : Guid.Parse(userId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cambiar(Guid id, EstadoVuelo nuevoEstado)
        {
            var result = await _sender.Send(new CambiarEstadoVueloCommand
            {
                VueloId = id,
                NuevoEstado = nuevoEstado,
                EjecutadorId = ObtenerEjecutadorId()
            });

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Estado actualizado correctamente.";
            }

            return RedirectToAction("Details", "Vuelos", new { id });
        }
    }
}
