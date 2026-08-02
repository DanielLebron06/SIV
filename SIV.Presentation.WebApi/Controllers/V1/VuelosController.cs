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
using SIV.Application.Features.Vuelos.Commands.CancelarVuelo;
using SIV.Application.Features.Vuelos.Commands.RegistrarRetraso;
using SIV.Application.Features.Vuelos.Commands.RegistrarCambioPuerta;
using SIV.Application.Features.Vuelos.Commands.RegistrarAdelanto;
using SIV.Application.Features.Vuelos.Queries.ConsultarHistorialCambios;
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

        [HttpGet("{id}/cambios")]
        public async Task<IActionResult> GetHistorialCambios(Guid id)
        {
            var result = await _sender.Send(new ConsultarHistorialCambiosQuery { VueloId = id });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id}/cancelar")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> Cancelar(Guid id, [FromBody] CancelarVueloDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new CancelarVueloCommand { VueloId = id, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            await _hubContext.Clients.All.SendAsync("VueloCancelado", id);
            return NoContent();
        }

        [HttpPut("{id}/retraso")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> RegistrarRetraso(Guid id, [FromBody] CambioOperativoTiempoDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarRetrasoCommand { VueloId = id, NuevaHoraEstimada = dto.NuevaHoraEstimada, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            await _hubContext.Clients.All.SendAsync("VueloRetrasado", id);
            return NoContent();
        }

        [HttpPut("{id}/adelanto")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> RegistrarAdelanto(Guid id, [FromBody] CambioOperativoTiempoDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarAdelantoCommand { VueloId = id, NuevaHoraEstimada = dto.NuevaHoraEstimada, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            await _hubContext.Clients.All.SendAsync("VueloAdelantado", id);
            return NoContent();
        }

        [HttpPut("{id}/puerta")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> RegistrarCambioPuerta(Guid id, [FromBody] CambioPuertaDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarCambioPuertaCommand { VueloId = id, NuevaPuerta = dto.NuevaPuerta, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            await _hubContext.Clients.All.SendAsync("VueloCambioPuerta", id);
            return NoContent();
        }
    }

    public class ActualizarEstadoDTO
    {
        public EstadoVuelo NuevoEstado { get; set; }
    }

    public class CancelarVueloDTO
    {
        public string Motivo { get; set; } = string.Empty;
    }

    public class CambioOperativoTiempoDTO
    {
        public DateTimeOffset NuevaHoraEstimada { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    public class CambioPuertaDTO
    {
        public string NuevaPuerta { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
    }
}