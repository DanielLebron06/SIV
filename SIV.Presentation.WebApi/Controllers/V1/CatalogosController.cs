using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Aerolinea;
using SIV.Application.DTOs.Aeropuerto;
using SIV.Application.Features.Vuelos.Commands.DesactivarAerolinea;
using SIV.Application.Features.Vuelos.Commands.DesactivarAeropuerto;
using SIV.Application.Features.Vuelos.Commands.RegistrarAerolinea;
using SIV.Application.Features.Vuelos.Commands.RegistrarAeropuerto;
using SIV.Application.Features.Vuelos.Queries.ObtenerAerolineas;
using SIV.Application.Features.Vuelos.Queries.ObtenerAeropuertos;
using SIV.Presentation.WebApi.Common;
using System.Security.Claims;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class CatalogosController : ControllerBase
    {
        private readonly ISender _sender;

        public CatalogosController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("aerolineas")]
        [ProducesResponseType(typeof(List<AerolineaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAerolineas()
        {
            var result = await _sender.Send(new ObtenerAerolineasQuery());
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpPost("aerolineas")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegistrarAerolinea([FromBody] RegistroAerolineaDTO datos)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarAerolineaCommand { Datos = datos, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return StatusCode(StatusCodes.Status201Created, ApiResponse.Ok("Aerolínea registrada exitosamente"));
        }

        [HttpPut("aerolineas/{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DesactivarAerolinea(Guid id)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new DesactivarAerolineaCommand { AerolineaId = id, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(ApiResponse.Ok("Aerolínea desactivada exitosamente"));
        }

        [HttpGet("aeropuertos")]
        [ProducesResponseType(typeof(List<AeropuertoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAeropuertos()
        {
            var result = await _sender.Send(new ObtenerAeropuertosQuery());
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpPost("aeropuertos")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegistrarAeropuerto([FromBody] RegistroAeropuertoDTO datos)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarAeropuertoCommand { Datos = datos, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return StatusCode(StatusCodes.Status201Created, ApiResponse.Ok("Aeropuerto registrado exitosamente"));
        }

        [HttpPut("aeropuertos/{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DesactivarAeropuerto(Guid id)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new DesactivarAeropuertoCommand { AeropuertoId = id, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(ApiResponse.Ok("Aeropuerto desactivado exitosamente"));
        }
    }
}
