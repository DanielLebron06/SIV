using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Notificacion;
using SIV.Application.Features.Notificaciones.Commands.MarcarNotificacionLeida;
using SIV.Application.Features.Notificaciones.Queries.ObtenerNotificacionesAdmin;
using SIV.Presentation.WebApi.Common;
using System;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class NotificacionesController : ControllerBase
    {
        private readonly ISender _sender;

        public NotificacionesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Administrador, Auditor")]
        [ProducesResponseType(typeof(List<NotificacionAdminDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetNotificacionesAdmin(
            [FromQuery] Guid? vueloId,
            [FromQuery] string? numeroVuelo,
            [FromQuery] Guid? usuarioId,
            [FromQuery] string? emailUsuario,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] bool? leida)
        {
            var result = await _sender.Send(new ObtenerNotificacionesAdminQuery
            {
                VueloId = vueloId,
                NumeroVuelo = numeroVuelo,
                UsuarioId = usuarioId,
                EmailUsuario = emailUsuario,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Leida = leida
            });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpPut("{id}/leida")]
        [Authorize(Roles = "UsuarioRegistrado, Administrador, Auditor")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarcarNotificacionLeida(Guid id)
        {
            var result = await _sender.Send(new MarcarNotificacionLeidaCommand { NotificacionId = id });
            if (!result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("No encontrada"))
                    return NotFound(ApiResponse.Error(result.Message));
                return BadRequest(ApiResponse.Error(result.Message));
            }
            return Ok(ApiResponse.Ok("Notificación marcada como leída"));
        }
    }
}
