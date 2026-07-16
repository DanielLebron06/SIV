using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Vuelo;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Queries.ObtenerEstadosVuelo
{
    /// <summary>
    /// Consulta para obtener el historial de estados de un vuelo específico.
    /// </summary>
    public class ObtenerEstadosVueloQuery : IRequest<Result<List<HistorialEstadoDTO>>>
    {
        public Guid VueloId { get; set; }
    }

    /// <summary>
    /// Validador para ObtenerEstadosVueloQuery.
    /// </summary>
    public class ObtenerEstadosVueloValidator : AbstractValidator<ObtenerEstadosVueloQuery>
    {
        public ObtenerEstadosVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("Se requiere el ID del vuelo.");
        }
    }

    /// <summary>
    /// Manejador para ObtenerEstadosVueloQuery.
    /// </summary>
    public class ObtenerEstadosVueloHandler : IRequestHandler<ObtenerEstadosVueloQuery, Result<List<HistorialEstadoDTO>>>
    {
        private readonly IHistorialEstadoRepository _historialEstadoRepository;
        private readonly ILogger<ObtenerEstadosVueloHandler> _logger;
        private readonly IValidator<ObtenerEstadosVueloQuery> _validator;

        public ObtenerEstadosVueloHandler(
            IHistorialEstadoRepository historialEstadoRepository,
            ILogger<ObtenerEstadosVueloHandler> logger,
            IValidator<ObtenerEstadosVueloQuery> validator)
        {
            _historialEstadoRepository = historialEstadoRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<HistorialEstadoDTO>>> Handle(ObtenerEstadosVueloQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<HistorialEstadoDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Uso de ToList() para materialización inmediata.
                var historial = (await _historialEstadoRepository.ObtenerPorVueloAsync(request.VueloId)).ToList();

                List<HistorialEstadoDTO> resultado = new();

                foreach (var estado in historial)
                {
                    resultado.Add(new HistorialEstadoDTO
                    {
                        Id = estado.Id,
                        Estado = estado.Estado,
                        FechaCambio = estado.FechaTransicion
                    });
                }

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener el historial de estados.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al consultar el historial de estados.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el historial de estados.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar el historial de estados del vuelo.";
                return result;
            }
        }
    }
}
