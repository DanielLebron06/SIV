using Microsoft.AspNetCore.Mvc;

namespace SIV.Presentation.WebApi.Controllers
{
    public class PublicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
