using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SIV.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 1. Registra de golpe todos los validadores (AbstractValidator) que creamos
        services.AddValidatorsFromAssembly(assembly);

        // 2. Registra MediatR y le dice que busque todos los Handlers en este proyecto
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        return services;
    }
}