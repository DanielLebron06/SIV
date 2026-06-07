using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIV.Infrastructure.Persistence.Context;

namespace SIV.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuramos la conexión a SQL Server
            services.AddDbContext<SIVDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SIVConnection"),
                    b => b.MigrationsAssembly(typeof(SIVDbContext).Assembly.FullName)));

            return services;
        }
    }
}