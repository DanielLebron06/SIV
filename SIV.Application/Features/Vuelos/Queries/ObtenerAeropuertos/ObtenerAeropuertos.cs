using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Aeropuerto;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Queries.ObtenerAeropuertos
{
    /// <summary>
    /// Consulta para obtener el listado de aeropuertos activos.
    /// </summary>
    public class ObtenerAeropuertosQuery : IRequest<Result<List<AeropuertoDTO>>>
    {
    }

    /// <summary>
    /// Validador para ObtenerAeropuertosQuery.
    /// </summary>
    public class ObtenerAeropuertosValidator : AbstractValidator<ObtenerAeropuertosQuery>
    {
        public ObtenerAeropuertosValidator()
        {
        }
    }

    /// <summary>
    /// Manejador para ObtenerAeropuertosQuery.
    /// </summary>
    public class ObtenerAeropuertosHandler : IRequestHandler<ObtenerAeropuertosQuery, Result<List<AeropuertoDTO>>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly ILogger<ObtenerAeropuertosHandler> _logger;
        private readonly IValidator<ObtenerAeropuertosQuery> _validator;

        public ObtenerAeropuertosHandler(
            IAeropuertoRepository aeropuertoRepository,
            ILogger<ObtenerAeropuertosHandler> logger,
            IValidator<ObtenerAeropuertosQuery> validator)
        {
            _aeropuertoRepository = aeropuertoRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<AeropuertoDTO>>> Handle(ObtenerAeropuertosQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<AeropuertoDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Uso de ToList() para materializar la consulta
                var aeropuertos = (await _aeropuertoRepository.ObtenerActivosAsync()).ToList();

                List<AeropuertoDTO> resultado = new();

                foreach (var aeropuerto in aeropuertos)
                {
                    resultado.Add(new AeropuertoDTO
                    {
                        Id = aeropuerto.Id,
                        Nombre = aeropuerto.Nombre,
                        CodigoIATA = aeropuerto.CodigoIATA,
                        Activo = aeropuerto.Activo
                    });
                }

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener los aeropuertos.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener los aeropuertos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener los aeropuertos.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar los aeropuertos.";
                return result;
            }
        }
    }
}
