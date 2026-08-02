using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.WebUser.Services;
using SIV.Presentation.WebUser.ViewModels;

namespace SIV.Presentation.WebUser.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ICuentaService _cuentaService;
        private readonly ISeguimientoService _seguimientoService;

        public CuentaController(ICuentaService cuentaService, ISeguimientoService seguimientoService)
        {
            _cuentaService = cuentaService;
            _seguimientoService = seguimientoService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("MisSeguimientos");
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                await _cuentaService.IniciarSesionAsync(modelo, modelo.Recordarme, cancellationToken);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MensajesError.ObtenerMensaje(ex));
                return View(modelo);
            }

            if (!string.IsNullOrWhiteSpace(modelo.ReturnUrl) && Url.IsLocalUrl(modelo.ReturnUrl))
            {
                return Redirect(modelo.ReturnUrl);
            }

            return RedirectToAction("MisSeguimientos");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View(new RegistroViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                await _cuentaService.RegistrarAsync(modelo, cancellationToken);
                TempData["Success"] = "Registro completado exitosamente. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MensajesError.ObtenerMensaje(ex));
                return View(modelo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _cuentaService.CerrarSesionAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MisSeguimientos(CancellationToken cancellationToken)
        {
            var seguimientos = await _seguimientoService.ObtenerSeguimientosAsync(cancellationToken);
            return View(seguimientos);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Notificaciones(CancellationToken cancellationToken)
        {
            var notificaciones = await _seguimientoService.ObtenerNotificacionesAsync(cancellationToken);
            return View(notificaciones);
        }
    }
}
