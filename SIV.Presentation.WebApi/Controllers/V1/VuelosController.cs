using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
using SIV.Presentation.WebApi.Common;
using SIV.Presentation.WebApi.DTOs;
using SIV.Presentation.WebApi.Hubs;
using System.Security.Claims;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
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
        [ProducesResponseType(typeof(List<DatosVueloDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] FiltrosVuelos filtros)
        {
            var result = await _sender.Send(new ConsultarVuelosQuery { Filtros = filtros });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DatosVueloDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _sender.Send(new ObtenerVueloQuery { VueloId = id });
            if (!result.Success) return NotFound(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] DatosVueloDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarVueloCommand { Datos = dto, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return StatusCode(StatusCodes.Status201Created, ApiResponse.Ok("Vuelo registrado exitosamente"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DatosVueloDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ActualizarVueloCommand { VueloId = id, Datos = dto, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(ApiResponse.Ok("Vuelo actualizado exitosamente"));
        }

        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEstado(Guid id, [FromBody] ActualizarEstadoDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new CambiarEstadoVueloCommand { VueloId = id, NuevoEstado = dto.NuevoEstado, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            await _hubContext.Clients.All.SendAsync("VueloEstadoCambiado", id, dto.NuevoEstado.ToString());
            return NoContent();
        }

        [HttpGet("{id}/historial")]
        [ProducesResponseType(typeof(List<HistorialEstadoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHistorial(Guid id)
        {
            var result = await _sender.Send(new ObtenerEstadosVueloQuery { VueloId = id });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpGet("{id}/cambios")]
        [ProducesResponseType(typeof(List<CambioOperativoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHistorialCambios(Guid id)
        {
            var result = await _sender.Send(new ConsultarHistorialCambiosQuery { VueloId = id });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpPut("{id}/cancelar")]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Cancelar(Guid id, [FromBody] CancelarVueloDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new CancelarVueloCommand { VueloId = id, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            await _hubContext.Clients.All.SendAsync("VueloCancelado", id);
            return NoContent();
        }

        [HttpPut("{id}/retraso")]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegistrarRetraso(Guid id, [FromBody] CambioOperativoTiempoDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarRetrasoCommand { VueloId = id, NuevaHoraEstimada = dto.NuevaHoraEstimada, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            await _hubContext.Clients.All.SendAsync("VueloRetrasado", id);
            return NoContent();
        }

        [HttpPut("{id}/adelanto")]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegistrarAdelanto(Guid id, [FromBody] CambioOperativoTiempoDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarAdelantoCommand { VueloId = id, NuevaHoraEstimada = dto.NuevaHoraEstimada, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            await _hubContext.Clients.All.SendAsync("VueloAdelantado", id);
            return NoContent();
        }

        [HttpPut("{id}/puerta")]
        [Authorize(Roles = "Operador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegistrarCambioPuerta(Guid id, [FromBody] CambioPuertaDTO dto)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarCambioPuertaCommand { VueloId = id, NuevaPuerta = dto.NuevaPuerta, Motivo = dto.Motivo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            await _hubContext.Clients.All.SendAsync("VueloCambioPuerta", id);
            return NoContent();
        }
    }
}
