using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Usuario;
using SIV.Application.Features.Usuarios.Commands.DejarSeguirVuelo;
using SIV.Application.Features.Usuarios.Commands.DesactivarUsuario;
using SIV.Application.Features.Usuarios.Commands.RegistrarUsuarioInterno;
using SIV.Application.Features.Usuarios.Commands.RegistrarUsuarioPublico;
using SIV.Application.Features.Usuarios.Commands.SeguirVuelo;
using SIV.Application.Features.Usuarios.Queries.ObtenerNotificaciones;
using SIV.Application.Features.Usuarios.Queries.ObtenerSeguidosDeUsuario;
using SIV.Application.Features.Usuarios.Queries.ObtenerUsuariosInternos;
using SIV.Application.DTOs.Seguimiento;
using SIV.Application.DTOs.Notificacion;
using SIV.Presentation.WebApi.Common;
using SIV.Presentation.WebApi.DTOs;
using System.Security.Claims;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly ISender _sender;

        public UsuariosController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("registro-publico")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistroPublico([FromBody] RegistroUsuarioDTO usuario)
        {
            var result = await _sender.Send(new RegistrarUsuarioPublicoCommand { Email = usuario.Email, Password = usuario.Password });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return StatusCode(StatusCodes.Status201Created, ApiResponse.Ok("Usuario registrado exitosamente como UsuarioRegistrado"));
        }

        [HttpPost("registro-interno")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegistroInterno([FromBody] RegistroUsuarioInternoDTO usuario)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarUsuarioInternoCommand { Email = usuario.Email, Password = usuario.Password, Rol = usuario.Rol, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return StatusCode(StatusCodes.Status201Created, ApiResponse.Ok("Usuario interno creado exitosamente"));
        }

        [HttpGet("seguimientos")]
        [Authorize(Roles = "UsuarioRegistrado")]
        [ProducesResponseType(typeof(List<SeguimientoVueloDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetSeguimientos()
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ObtenerSeguidosDeUsuarioQuery { UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpGet("notificaciones")]
        [Authorize(Roles = "UsuarioRegistrado, Administrador, Auditor")]
        [ProducesResponseType(typeof(List<NotificacionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetNotificaciones()
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ObtenerNotificacionesQuery { UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpPost("seguimiento")]
        [Authorize(Roles = "UsuarioRegistrado")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AgregarSeguimiento([FromBody] AgregarSeguimientoRequest request)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new SeguirVueloCommand { VueloId = request.VueloId, UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return StatusCode(StatusCodes.Status201Created, ApiResponse.Ok("Seguimiento agregado exitosamente"));
        }

        [HttpDelete("seguimiento/{vueloId}")]
        [Authorize(Roles = "UsuarioRegistrado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DejarSeguirVuelo(Guid vueloId)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new DejarSeguirVueloCommand { VueloId = vueloId, UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return NoContent();
        }

        [HttpGet("internos")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(List<UsuarioInternoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsuariosInternos()
        {
            var result = await _sender.Send(new ObtenerUsuariosInternosQuery());

            if (!result.Success)
                return BadRequest(ApiResponse.Error(result.Message));

            return Ok(result.Data);
        }

        [HttpPut("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DesactivarUsuario(Guid id)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new DesactivarUsuarioCommand { IdUsuarioADesactivar = id, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(ApiResponse.Ok("Usuario desactivado exitosamente"));
        }
        
    }
}
