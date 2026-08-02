using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Notificacion;
using SIV.Application.DTOs.Seguimiento;
using SIV.Application.DTOs.Usuario;
using SIV.Application.Features.Usuarios.Commands.ActivarUsuario;
using SIV.Application.Features.Usuarios.Commands.DejarSeguirVuelo;
using SIV.Application.Features.Usuarios.Commands.DesactivarUsuario;
using SIV.Application.Features.Usuarios.Commands.RegistrarUsuarioInterno;
using SIV.Application.Features.Usuarios.Commands.RegistrarUsuarioPublico;
using SIV.Application.Features.Usuarios.Commands.SeguirVuelo;
using SIV.Application.Features.Usuarios.Queries.ObtenerNotificaciones;
using SIV.Application.Features.Usuarios.Queries.ObtenerSeguidosDeUsuario;
using SIV.Application.Features.Usuarios.Queries.ObtenerUsuariosInternos;
using SIV.Domain.Emuns;
using System.Security.Claims;

namespace SIV.Presentation.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ISender _sender;

        private static readonly Guid UsuarioPruebasId =
            Guid.Parse("352717f7-14e6-4dfe-a183-aeaa21717ae3");

        private Guid ObtenerUsuarioActual()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return UsuarioPruebasId;

            return Guid.Parse(userId);
        }

        public UsuariosController(ISender sender)
        {
            _sender = sender;
        }


        // GET: Usuarios
        [HttpGet]
        public async Task<IActionResult> Index(bool? activo)
        {
            var result = await _sender.Send(
                new ObtenerUsuariosInternosQuery
                {
                    Activo = activo
                });

            ViewBag.Activo = activo;

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View(Enumerable.Empty<UsuarioInternoDTO>());
            }

            return View(result.Data);
        }

        // POST: Usuarios/Desactivar/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var result = await _sender.Send(new DesactivarUsuarioCommand
            {
                IdUsuarioADesactivar = id,
                EjecutadorId = ObtenerUsuarioActual()
            });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] = "Usuario desactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Usuarios/Activar/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(Guid id)
        {
            var result = await _sender.Send(
                new ActivarUsuarioCommand
                {
                    IdUsuarioAactivar = id,
                    EjecutadorId = ObtenerUsuarioActual()
                });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] = "Usuario activado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Usuarios/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistroUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _sender.Send(new RegistrarUsuarioPublicoCommand
            {
                Email = dto.Email,
                Password = dto.Password
            });

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            TempData["Success"] =
                "Usuario registrado exitosamente.";

            return RedirectToAction(nameof(Register));
        }

        // GET: Usuarios/RegisterInterno
        [HttpGet]
        public IActionResult RegisterInterno()
        {
            ViewBag.Roles = Enum.GetValues<Rol>()
            .Where(r => r != Rol.UsuarioRegistrado);

            return View();

        }

        // POST: Usuarios/RegisterInterno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterInterno(
            RegistroUsuarioInternoDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result =
                await _sender.Send(new RegistrarUsuarioInternoCommand
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    Rol = dto.Rol,
                    EjecutadorId = ObtenerUsuarioActual()
                });

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(dto);
            }

            TempData["Success"] =
                "Usuario interno registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Seguimientos
        [HttpGet]
        public async Task<IActionResult> Seguimientos()
        {
            var result =
                await _sender.Send(new ObtenerSeguidosDeUsuarioQuery
                {
                    UsuarioId = ObtenerUsuarioActual()
                });

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View(Enumerable.Empty<SeguimientoVueloDTO>());
            }

            return View(result.Data);
        }

        // POST: Usuarios/SeguirVuelo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeguirVuelo(Guid vueloId)
        {
            var result =
                await _sender.Send(new SeguirVueloCommand
                {
                    VueloId = vueloId,
                    UsuarioId = ObtenerUsuarioActual()
                });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] =
                    "Ahora estás siguiendo este vuelo.";

            return RedirectToAction(
                "Details",
                "Vuelos",
                new { id = vueloId });
        }

        // POST: Usuarios/DejarSeguir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DejarSeguir(Guid vueloId)
        {
            var result =
                await _sender.Send(new DejarSeguirVueloCommand
                {
                    VueloId = vueloId,
                    UsuarioId = ObtenerUsuarioActual()
                });

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["Success"] =
                    "Has dejado de seguir el vuelo.";

            return RedirectToAction(nameof(Seguimientos));
        }

        // GET: Usuarios/Notificaciones
        [HttpGet]
        public async Task<IActionResult> Notificaciones()
        {
            var result =
                await _sender.Send(new ObtenerNotificacionesQuery
                {
                    UsuarioId = ObtenerUsuarioActual()
                });

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View(Enumerable.Empty<NotificacionDTO>());
            }

            return View(result.Data);
        }

    }
}