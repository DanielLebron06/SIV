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
using System.Security.Claims;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ISender _sender;

        public UsuariosController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("registro-publico")]
        public async Task<IActionResult> RegistroPublico([FromBody] RegistroUsuarioDTO usuario)
        {
            var result = await _sender.Send(new RegistrarUsuarioPublicoCommand { Email = usuario.Email, Password = usuario.Password });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { mensaje = "Usuario registrado exitosamente como UsuarioRegistrado" });
        }

        [HttpPost("registro-interno")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistroInterno([FromBody] RegistroUsuarioInternoDTO usuario)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new RegistrarUsuarioInternoCommand { Email = usuario.Email, Password = usuario.Password, Rol = usuario.Rol, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { mensaje = "Usuario interno creado exitosamente" });
        }

        [HttpGet("seguimientos")]
        [Authorize(Roles = "UsuarioRegistrado")]
        public async Task<IActionResult> GetSeguimientos()
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ObtenerSeguidosDeUsuarioQuery { UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("notificaciones")]
        [Authorize(Roles = "UsuarioRegistrado, Administrador, Auditor")]
        public async Task<IActionResult> GetNotificaciones()
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ObtenerNotificacionesQuery { UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPost("seguimiento")]
        [Authorize(Roles = "UsuarioRegistrado")]
        public async Task<IActionResult> AgregarSeguimiento([FromBody] AgregarSeguimientoRequest request)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new SeguirVueloCommand { VueloId = request.VueloId, UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpDelete("seguimiento/{vueloId}")]
        [Authorize(Roles = "UsuarioRegistrado")]
        public async Task<IActionResult> DejarSeguirVuelo(Guid vueloId)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new DejarSeguirVueloCommand { VueloId = vueloId, UsuarioId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPut("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DesactivarUsuario(Guid id)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new DesactivarUsuarioCommand { IdUsuarioADesactivar = id, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { mensaje = "Usuario desactivado exitosamente" });
        }
    }

    public class AgregarSeguimientoRequest
    {
        public Guid VueloId { get; set; }
    }
}
