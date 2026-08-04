using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SIV.Presentation.WebUser.ViewModels;

namespace SIV.Presentation.WebUser.Services
{
    public interface ICuentaService
    {
        Task<string> IniciarSesionAsync(LoginViewModel login, bool recordarme, CancellationToken cancellationToken = default);
        Task<string> RegistrarAsync(RegistroViewModel registro, CancellationToken cancellationToken = default);
        Task CerrarSesionAsync();
    }

    public class CuentaService : ICuentaService
    {
        private readonly IWebApiClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CuentaService(IWebApiClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> IniciarSesionAsync(LoginViewModel login, bool recordarme, CancellationToken cancellationToken = default)
        {
            var token = await _client.LoginAsync(login, cancellationToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, login.Email),
                new(ClaimTypes.Role, "UsuarioRegistrado"),
                new("Token", token)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = recordarme });

            return token;
        }

        public Task<string> RegistrarAsync(RegistroViewModel registro, CancellationToken cancellationToken = default)
        {
            return _client.RegistroAsync(registro, cancellationToken);
        }

        public Task CerrarSesionAsync()
        {
            return _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
