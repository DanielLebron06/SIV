using Microsoft.AspNetCore.Mvc;
using SIV.Application.Service.Interfaces;
using SIV.Domain.Entities;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogosController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public CatalogosController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        // GET /api/v1/Catalogos/aerolineas
        [HttpGet("aerolineas")]
        public async Task<IActionResult> GetAerolineas()
        {
            Usuario usuarioContext = new Usuario { Email = "public@siv.com" };
            var aerolineas = await _flightService.ObtenerAerolineas(usuarioContext);
            return Ok(aerolineas);
        }

        // GET /api/v1/Catalogos/aeropuertos
        [HttpGet("aeropuertos")]
        public async Task<IActionResult> GetAeropuertos()
        {
            Usuario usuarioContext = new Usuario { Email = "public@siv.com" };
            var aeropuertos = await _flightService.ObtenerAeropuertos(usuarioContext);
            return Ok(aeropuertos);
        }
    }
}
