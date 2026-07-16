using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SIV.Application.DTOs.Vuelo;
using SIV.Application.Service.Interfaces;
using SIV.Domain.Entities;
using SIV.Domain.Emuns;
using SIV.Domain.Common;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VuelosController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IUserService _userService;

        public VuelosController(IFlightService flightService, IUserService userService)
        {
            _flightService = flightService;
            _userService = userService;
        }

        // GET /api/v1/Vuelos
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FiltrosVuelos filtros)
        {
            Usuario usuarioContext = User.Identity?.IsAuthenticated == true
                ? await _userService.ObtenerPorEmail(User.Identity.Name)
                : new Usuario { Email = "public@siv.com" };

            var resultado = await _flightService.ConsultarVuelos(filtros, usuarioContext);
            return Ok(resultado);
        }

        // GET /api/v1/Vuelos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                
                var vuelo = await _flightService.ObtenerVuelo(id);

                if (vuelo == null)
                {
                    return NotFound(new { mensaje = $"No se encontró un vuelo con el ID {id}" });
                }

                return Ok(vuelo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST /api/v1/Vuelos
        [HttpPost]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> Create([FromBody] DatosVueloDTO dto)
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity.Name);
            await _flightService.RegistrarVuelo(dto, usuarioEjecutador);
            return Ok();
        }

        // PUT /api/v1/Vuelos/{id}/estado
        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> UpdateEstado(Guid id, [FromBody] ActualizarEstadoDTO dto)
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity.Name);
            await _flightService.CambiarEstadoVuelo(id, dto.NuevoEstado, usuarioEjecutador);
            return NoContent();
        }

        // GET /api/v1/Vuelos/{id}/historial
        [HttpGet("{id}/historial")]
        public async Task<IActionResult> GetHistorial(Guid id)
        {
            Usuario usuarioContext = User.Identity?.IsAuthenticated == true
                ? await _userService.ObtenerPorEmail(User.Identity.Name)
                : new Usuario { Email = "public@siv.com" };

            var historial = await _flightService.ObtenerEstadosVuelo(id, usuarioContext);
            return Ok(historial);
        }
    }

    public class ActualizarEstadoDTO
    {
        public EstadoVuelo NuevoEstado { get; set; }
    }
}