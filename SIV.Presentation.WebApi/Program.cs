using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIV.Application;
using SIV.Application.Auditoria;
using SIV.Application.Service.Implementations;
using SIV.Application.Service.Interfaces;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using SIV.Infrastructure.Persistence;
using SIV.Infrastructure.Persistence.Repositorios;
using SIV.Infrastructure.Persistence.UnitOfWork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("SIVTestConnection");
builder.Services.AddDbContext<SIVDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SIV.Infrastructure")));


builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportesService, ReportesService>();


// Auditoría
builder.Services.AddScoped<IAuditoriaManager, AuditoriaManager>();
builder.Services.AddScoped<IBaseRepository<LogAuditoria>, BaseRepository<LogAuditoria>>();

// Repositorios (Aquí es donde estaba el problema principal)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICambioOperativoRepository, CambioOperativoRepository>();
builder.Services.AddScoped<IVueloRepository, VueloRepository>();
builder.Services.AddScoped<IHistorialEstadoRepository, HistorialEstadoRepository>();

// Repositorios adicionales que seguramente necesitarás pronto:
builder.Services.AddScoped<IAerolineaRepository, AerolineaRepository>();
builder.Services.AddScoped<IAeropuertoRepository, AeropuertoRepository>();
builder.Services.AddScoped<ISeguimientoVueloRepository, SeguimientoVueloRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<ILogAuditoriaRepository, LogAuditoriaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registramos los servicios que acabamos de configurar en la capa de Application
builder.Services.AddApplicationServices();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SIV API", Version = "v1" });

    // Configuración para el botón Authorize con JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT. Pega tu token JWT directamente en la caja de texto inferior.",
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

// Configuración de JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Debe ir antes que UseAuthorization
app.UseAuthorization();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();
app.Run();