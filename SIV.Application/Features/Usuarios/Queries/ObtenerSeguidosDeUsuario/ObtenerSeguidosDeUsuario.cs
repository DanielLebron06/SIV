using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Seguimiento;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Queries.ObtenerSeguidosDeUsuario
{
    /// <summary>
    /// Consulta para obtener los vuelos que está siguiendo un usuario.
    /// </summary>
    public class ObtenerSeguidosDeUsuarioQuery : IRequest<Result<List<SeguimientoVueloDTO>>>
    {
        public Guid UsuarioId { get; set; }
    }

    /// <summary>
    /// Validador para ObtenerSeguidosDeUsuarioQuery.
    /// </summary>
    public class ObtenerSeguidosDeUsuarioValidator : AbstractValidator<ObtenerSeguidosDeUsuarioQuery>
    {
        public ObtenerSeguidosDeUsuarioValidator()
        {
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Se requiere el ID del usuario.");
        }
    }

    /// <summary>
    /// Manejador para ObtenerSeguidosDeUsuarioQuery.
    /// </summary>
    public class ObtenerSeguidosDeUsuarioHandler : IRequestHandler<ObtenerSeguidosDeUsuarioQuery, Result<List<SeguimientoVueloDTO>>>
    {
        private readonly ISeguimientoVueloRepository _seguimientoVueloRepository;
        private readonly ILogger<ObtenerSeguidosDeUsuarioHandler> _logger;
        private readonly IValidator<ObtenerSeguidosDeUsuarioQuery> _validator;

        public ObtenerSeguidosDeUsuarioHandler(
            ISeguimientoVueloRepository seguimientoVueloRepository,
            ILogger<ObtenerSeguidosDeUsuarioHandler> logger,
            IValidator<ObtenerSeguidosDeUsuarioQuery> validator)
        {
            _seguimientoVueloRepository = seguimientoVueloRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<SeguimientoVueloDTO>>> Handle(ObtenerSeguidosDeUsuarioQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<SeguimientoVueloDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Se materializa la consulta para evitar ejecución diferida.
                var seguimientos = (await _seguimientoVueloRepository.BuscarActivosPorUsuario(request.UsuarioId)).ToList();

                List<SeguimientoVueloDTO> listadoDTO = new();

                foreach (var seguimiento in seguimientos)
                {
                    listadoDTO.Add(new SeguimientoVueloDTO
                    {
                        SeguimientoId = seguimiento.Id,
                        VueloId = seguimiento.VueloId,
                        NumeroVuelo = seguimiento.Vuelo?.NumeroVuelo,
                        FechaInicio = seguimiento.FechaInicio,
                        FechaFin = seguimiento.FechaFin
                    });
                }

                result.Success = true;
                result.Data = listadoDTO;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener los vuelos seguidos.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener los vuelos seguidos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener los vuelos seguidos.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar los seguimientos.";
                return result;
            }
        }
    }
}
