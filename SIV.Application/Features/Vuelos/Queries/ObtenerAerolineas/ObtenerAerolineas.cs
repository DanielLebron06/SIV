using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Aerolinea;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Queries.ObtenerAerolineas
{
    /// <summary>
    /// Consulta para obtener el listado de aerolíneas activas.
    /// </summary>
    public class ObtenerAerolineasQuery : IRequest<Result<List<AerolineaDTO>>>
    {
    }

    /// <summary>
    /// Validador para ObtenerAerolineasQuery.
    /// </summary>
    public class ObtenerAerolineasValidator : AbstractValidator<ObtenerAerolineasQuery>
    {
        public ObtenerAerolineasValidator()
        {
        }
    }

    /// <summary>
    /// Manejador para ObtenerAerolineasQuery.
    /// </summary>
    public class ObtenerAerolineasHandler : IRequestHandler<ObtenerAerolineasQuery, Result<List<AerolineaDTO>>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly ILogger<ObtenerAerolineasHandler> _logger;
        private readonly IValidator<ObtenerAerolineasQuery> _validator;

        public ObtenerAerolineasHandler(
            IAerolineaRepository aerolineaRepository,
            ILogger<ObtenerAerolineasHandler> logger,
            IValidator<ObtenerAerolineasQuery> validator)
        {
            _aerolineaRepository = aerolineaRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<AerolineaDTO>>> Handle(ObtenerAerolineasQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<AerolineaDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Uso de ToList() para materializar inmediatamente
                var aerolineas = (await _aerolineaRepository.ObtenerActivosAsync()).ToList();

                List<AerolineaDTO> resultado = new();

                foreach (var aerolinea in aerolineas)
                {
                    resultado.Add(new AerolineaDTO
                    {
                        Id = aerolinea.Id,
                        Nombre = aerolinea.Nombre,
                        Activa = aerolinea.Activo
                    });
                }

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener las aerolíneas.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener las aerolíneas.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener las aerolíneas.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar las aerolíneas.";
                return result;
            }
        }
    }
}
