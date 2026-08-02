using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SIV.Application.Auditoria;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using SIV.Infrastructure.Persistence;
using SIV.Infrastructure.Persistence.Repositorios;
using SIV.Infrastructure.Persistence.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(SIV.Application.Features.Vuelos.Queries.ConsultarVuelos.ConsultarVuelosQuery).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(SIV.Application.Features.Vuelos.Queries.ConsultarVuelos.ConsultarVuelosQuery).Assembly);

builder.Services.AddDbContext<SIVDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIVConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());


builder.Services.AddScoped<IVueloRepository, VueloRepository>();
builder.Services.AddScoped<IAuditoriaManager, AuditoriaManager>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBaseRepository<LogAuditoria>, BaseRepository<LogAuditoria>>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICambioOperativoRepository, CambioOperativoRepository>();
builder.Services.AddScoped<IVueloRepository, VueloRepository>();
builder.Services.AddScoped<IHistorialEstadoRepository, HistorialEstadoRepository>();
builder.Services.AddScoped<IAerolineaRepository, AerolineaRepository>();
builder.Services.AddScoped<IAeropuertoRepository, AeropuertoRepository>();
builder.Services.AddScoped<ISeguimientoVueloRepository, SeguimientoVueloRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<ILogAuditoriaRepository, LogAuditoriaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
