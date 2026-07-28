using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SIV.Application.DTOs.Vuelo;
using SIV.Application.Features.Vuelos.Commands.ActualizarVuelo;
using SIV.Application.Features.Vuelos.Commands.CambiarEstadoVuelo;
using SIV.Application.Features.Vuelos.Commands.RegistrarVuelo;
using SIV.Application.Features.Vuelos.Queries.ConsultarVuelos;
using SIV.Application.Features.Vuelos.Queries.ObtenerEstadosVuelo;
using SIV.Application.Features.Vuelos.Queries.ObtenerVuelo;
using SIV.Domain.Common;
using SIV.Domain.Emuns;
using SIV.Presentation.WebApi.Hubs;
using System.Security.Claims;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VuelosController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHubContext<VuelosHub> _hubContext;
        public VuelosController(ISender sender, IHubContext<VuelosHub> hubContext)
        {
            _sender = sender;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FiltrosVuelos filtros)
        {
            var result = await _sender.Send(new ConsultarVuelosQuery { Filtros = filtros });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _sender.Send(new ObtenerVueloQuery { VueloId = id });
            if (!result.Success) return NotFound(new { mensaje = result.Message });
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> Create([FromBody] DatosVueloDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarVueloCommand { Datos = dto, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DatosVueloDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ActualizarVueloCommand { VueloId = id, Datos = dto, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> UpdateEstado(Guid id, [FromBody] ActualizarEstadoDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new CambiarEstadoVueloCommand { VueloId = id, NuevoEstado = dto.NuevoEstado, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            await _hubContext.Clients.All.SendAsync("VueloEstadoCambiado", id, dto.NuevoEstado.ToString());
            return NoContent();
        }

        [HttpGet("{id}/historial")]
        public async Task<IActionResult> GetHistorial(Guid id)
        {
            var result = await _sender.Send(new ObtenerEstadosVueloQuery { VueloId = id });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }

    public class ActualizarEstadoDTO
    {
        public EstadoVuelo NuevoEstado { get; set; }
    }
}