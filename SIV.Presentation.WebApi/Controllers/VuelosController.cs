using Microsoft.AspNetCore.Mvc;

namespace SIV.Presentation.WebApi.Controllers
{
    public class VuelosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
