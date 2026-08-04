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
using SIV.Domain.Common;

namespace SIV.Application.Features.Vuelos.Queries.ConsultarVuelos
{
    /// <summary>
    /// Consulta para buscar vuelos en base a filtros especificados.
    /// </summary>
    public class ConsultarVuelosQuery : IRequest<Result<List<DatosVueloDTO>>>
    {
        public FiltrosVuelos? Filtros { get; set; } = new();
    }

    /// <summary>
    /// Validador para ConsultarVuelosQuery.
    /// </summary>
    public class ConsultarVuelosValidator : AbstractValidator<ConsultarVuelosQuery>
    {
        public ConsultarVuelosValidator()
        {
            RuleFor(x => x.Filtros).NotNull().WithMessage("Los filtros no pueden ser nulos.");
        }
    }

    /// <summary>
    /// Manejador para ConsultarVuelosQuery.
    /// </summary>
    public class ConsultarVuelosHandler : IRequestHandler<ConsultarVuelosQuery, Result<List<DatosVueloDTO>>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ILogger<ConsultarVuelosHandler> _logger;
        private readonly IValidator<ConsultarVuelosQuery> _validator;

        public ConsultarVuelosHandler(
            IVueloRepository vueloRepository,
            ILogger<ConsultarVuelosHandler> logger,
            IValidator<ConsultarVuelosQuery> validator)
        {
            _vueloRepository = vueloRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<DatosVueloDTO>>> Handle(ConsultarVuelosQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<DatosVueloDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Uso de ToList() para materializar inmediatamente la colección.
                var vuelos = (await _vueloRepository.BuscarConFiltros(request.Filtros)).ToList();

                List<DatosVueloDTO> resultado = new();

                foreach (var vuelo in vuelos)
                {
                    resultado.Add(new DatosVueloDTO
                    {
                        Id = vuelo.Id,
                        NumeroVuelo = vuelo.NumeroVuelo,
                        AerolineaId = vuelo.AerolineaId,
                        AeropuertoOrigenId = vuelo.AeropuertoOrigenId,
                        AeropuertoDestinoId = vuelo.AeropuertoDestinoId,
                        AerolineaNombre = vuelo.Aerolinea?.Nombre ?? "N/A",
                        AeropuertoOrigenIATA = vuelo.AeropuertoOrigen?.CodigoIATA ?? "N/A",
                        AeropuertoDestinoIATA = vuelo.AeropuertoDestino?.CodigoIATA ?? "N/A",
                        EstadoActual = vuelo.EstadoActual,
                        PuertaEmbarque = vuelo.PuertaEmbarque,
                        FechaSalidaProgramada = vuelo.SalidaPlanificada,
                        FechaLlegadaProgramada = vuelo.LlegadaPlanificada
                    });
                }

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al consultar los vuelos.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al buscar los vuelos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consultar los vuelos.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al realizar la consulta de vuelos.";
                return result;
            }
        }
    }
}
