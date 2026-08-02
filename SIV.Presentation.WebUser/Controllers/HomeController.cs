using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.WebUser.ViewModels;

namespace SIV.Presentation.WebUser.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode, string? mensaje)
        {
            var modelo = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode ?? StatusCodes.Status500InternalServerError,
                Mensaje = string.IsNullOrWhiteSpace(mensaje) ? "Ocurrió un error inesperado." : mensaje
            };
            return View(modelo);
        }
    }
}
