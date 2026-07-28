using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Vuelo;
using SIV.Application.Features.Vuelos.Queries.ConsultarVuelos;
using SIV.Domain.Common;

namespace SIV.Presentation.Web.Controllers
{
    public class TableroVuelosController : Controller
    {
        private readonly ISender _sender;

        public TableroVuelosController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] FiltrosVuelos filtros)
        {
            filtros ??= new FiltrosVuelos();
            var result = await _sender.Send(new ConsultarVuelosQuery { Filtros = filtros });

            ViewBag.Filtros = filtros;
            return View(result.Success ? result.Data : Enumerable.Empty<DatosVueloDTO>());
        }
    }
}