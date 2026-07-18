using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIV.Application;
using SIV.Application.Auditoria;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using SIV.Infrastructure.Persistence;
using SIV.Infrastructure.Persistence.Repositorios;
using SIV.Infrastructure.Persistence.UnitOfWork;
using System.Text;

namespace SIV.Presentation.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSignalR();
        services.AddEndpointsApiExplorer();

        var connectionString = configuration.GetConnectionString("SIVTestConnection");
        services.AddDbContext<SIVDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SIV.Infrastructure.Persistence")));

        services.AddScoped<IAuditoriaManager, AuditoriaManager>();
        services.AddScoped<IBaseRepository<LogAuditoria>, BaseRepository<LogAuditoria>>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICambioOperativoRepository, CambioOperativoRepository>();
        services.AddScoped<IVueloRepository, VueloRepository>();
        services.AddScoped<IHistorialEstadoRepository, HistorialEstadoRepository>();
        services.AddScoped<IAerolineaRepository, AerolineaRepository>();
        services.AddScoped<IAeropuertoRepository, AeropuertoRepository>();
        services.AddScoped<ISeguimientoVueloRepository, SeguimientoVueloRepository>();
        services.AddScoped<INotificacionRepository, NotificacionRepository>();
        services.AddScoped<ILogAuditoriaRepository, LogAuditoriaRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddApplicationServices();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SIV API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        var jwtKey = configuration["Jwt:Key"];
        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                };
            });

        services.AddAuthorization();

        return services;
    }
}
