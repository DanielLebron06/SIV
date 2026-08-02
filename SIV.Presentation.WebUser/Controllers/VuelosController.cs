using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Presentation.WebUser.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SIV.Presentation.WebUser.Controllers
{
    public class VuelosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public VuelosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // Helper para crear cliente HTTP con Token si existe
        private HttpClient CrearClienteConToken()
        {
            var client = _httpClientFactory.CreateClient("SivApi");
            var token = User.FindFirst("JWTToken")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        // GET: /Vuelos
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("SivApi");
            var response = await client.GetAsync("vuelos");

            List<VueloViewModel> vuelos = new();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                vuelos = JsonSerializer.Deserialize<List<VueloViewModel>>(content, _jsonOptions) ?? new();
            }

            return View(vuelos);
        }

        // GET: /Vuelos/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("MisSeguimientos");

            return View();
        }

        // POST: /Vuelos/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("SivApi");
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);

                // Crear sesión de cookie guardando el JWT
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim("JWTToken", result?.Token ?? string.Empty)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("MisSeguimientos");
            }

            ViewBag.Error = "Credenciales inválidas o error de autenticación.";
            return View(model);
        }

        // POST: /Vuelos/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid) return View("Login");

            var client = _httpClientFactory.CreateClient("SivApi");
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("usuarios/registro-publico", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registro completado exitosamente. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            var errorContent = await response.Content.ReadAsStringAsync();

            ViewBag.Error = !string.IsNullOrEmpty(errorContent)
                ? errorContent
                : "No se pudo completar el registro. Verifica los datos ingresados.";

            ViewBag.ActiveTab = "registro";
            return View("Login");
        }

        // GET: /Vuelos/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        // GET: /Vuelos/MisSeguimientos
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MisSeguimientos()
        {
            var client = CrearClienteConToken();
            var response = await client.GetAsync("usuarios/seguimientos");

            List<VueloViewModel> seguimientos = new();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                seguimientos = JsonSerializer.Deserialize<List<VueloViewModel>>(content, _jsonOptions) ?? new();
            }

            return View(seguimientos);
        }

        // POST: /Vuelos/Seguir
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguir(Guid vueloId)
        {
            var client = CrearClienteConToken();
            var json = JsonSerializer.Serialize(new { vueloId });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("usuarios/seguimiento", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Vuelo agregado a tus seguimientos.";
            }
            else
            {
                TempData["Error"] = "No se pudo seguir el vuelo.";
            }

            return RedirectToAction("Index");
        }

        // POST: /Vuelos/DejarDeSeguir
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DejarDeSeguir(Guid vueloId)
        {
            var client = CrearClienteConToken();
            var response = await client.DeleteAsync($"usuarios/seguimiento/{vueloId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Vuelo eliminado de tus seguimientos.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el seguimiento.";
            }

            return RedirectToAction("MisSeguimientos");
        }
    }
}