using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Usuario;
using SIV.Application.Service.Interfaces;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsuariosController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("registro-publico")]
        public async Task<IActionResult> RegistroPublico([FromBody] RegistroUsuarioDTO usuario)
        {
            await _userService.RegistraUsuarioPublico(usuario);
            return Ok(new { mensaje = "Usuario registrado exitosamente como UsuarioRegistrado" });
        }

        [HttpPost("registro-interno")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistroInterno([FromBody] RegistroUsuarioInternoDTO usuario)
        {
            // 1. Obtenemos el email del token
            var emailEjecutador = User.Identity.Name;

            // 2. El controlador le pide al SERVICIO, no al repositorio
            var usuarioEjecutador = await _userService.ObtenerPorEmail(emailEjecutador);

            // 3. Ejecutamos la lógica de negocio
            await _userService.RegistraUsuarioInterno(usuario, usuarioEjecutador);

            return Ok(new { mensaje = "Usuario interno creado exitosamente" });
        }

        // GET /api/v1/Usuarios/seguimientos
        [HttpGet("seguimientos")]
        [Authorize(Roles = "UsuarioRegistrado")]
        public async Task<IActionResult> GetSeguimientos()
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity?.Name ?? string.Empty);
            var seguimientos = await _userService.ObtenerSeguidosDeUsuario(usuarioEjecutador);
            return Ok(seguimientos);
        }

        // GET /api/v1/Usuarios/notificaciones
        [HttpGet("notificaciones")]
        [Authorize(Roles = "UsuarioRegistrado, Administrador, Auditor")]
        public async Task<IActionResult> GetNotificaciones()
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity?.Name ?? string.Empty);
            var notificaciones = await _userService.ObtnerNotificaciones(usuarioEjecutador);
            return Ok(notificaciones);
        }

        // POST /api/v1/Usuarios/seguimiento
        [HttpPost("seguimiento")]
        [Authorize(Roles = "UsuarioRegistrado")]
        public async Task<IActionResult> AgregarSeguimiento([FromBody] AgregarSeguimientoRequest request)
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity?.Name ?? string.Empty);
            await _userService.SeguirVuelo(request.VueloId, usuarioEjecutador);
            return Ok();
        }
    }

    public class AgregarSeguimientoRequest
    {
        public Guid VueloId { get; set; }
    }
}
