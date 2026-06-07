using Microsoft.AspNetCore.Mvc;

namespace SIV.Presentation.WebApi.Controllers
{
    public class CambiosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
