using SIV.Presentation.FIDS.Services;

namespace SIV.Presentation.FIDS;

/// <summary>
/// Métodos de extensión para registrar los servicios del proyecto de presentación FIDS.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddFidsPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.AddHttpClient<IFidsApiClient, FidsApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        return services;
    }
}
