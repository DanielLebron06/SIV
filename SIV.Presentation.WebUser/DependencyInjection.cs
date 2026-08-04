using Microsoft.AspNetCore.Authentication.Cookies;
using SIV.Presentation.WebUser.Services.Common;
using SIV.Presentation.WebUser.Services.Cuenta;
using SIV.Presentation.WebUser.Services.Seguimiento;
using SIV.Presentation.WebUser.Services.Vuelos;

namespace SIV.Presentation.WebUser;

/// <summary>
/// Métodos de extensión para registrar los servicios del proyecto de presentación WebUser.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddWebUserPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.AddHttpClient<IWebApiClient, WebApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<IPublicVueloService, PublicVueloService>();
        services.AddScoped<ICuentaService, CuentaService>();
        services.AddScoped<ISeguimientoService, SeguimientoService>();

        services.AddHttpContextAccessor();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Cuenta/Login";
                options.LogoutPath = "/Cuenta/Logout";
                options.AccessDeniedPath = "/Cuenta/Login";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });

        return services;
    }
}
