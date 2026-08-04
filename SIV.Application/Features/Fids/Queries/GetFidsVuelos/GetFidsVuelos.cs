using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Fids;
using SIV.Domain.Common;
using SIV.Domain.Emuns;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Fids.Queries.GetFidsVuelos
{
    /// <summary>
    /// Consulta para obtener los vuelos que se muestran en un tablero FIDS.
    /// </summary>
    public class GetFidsVuelosQuery : IRequest<Result<List<DtoFidsVuelo>>>
    {
        public TipoPantallaFids TipoPantalla { get; set; } = TipoPantallaFids.General;
        public string? AeropuertoCodigo { get; set; }
        public EstadoVuelo? Estado { get; set; }
        public Guid? AerolineaId { get; set; }
        public TimeSpan? RangoHoras { get; set; }
    }

    /// <summary>
    /// Validador para GetFidsVuelosQuery.
    /// </summary>
    public class GetFidsVuelosValidator : AbstractValidator<GetFidsVuelosQuery>
    {
        public GetFidsVuelosValidator()
        {
            RuleFor(x => x.TipoPantalla).IsInEnum().WithMessage("El tipo de pantalla no es válido.");
            RuleFor(x => x.RangoHoras).GreaterThan(TimeSpan.Zero).When(x => x.RangoHoras.HasValue)
                .WithMessage("El rango de horas debe ser mayor a cero.");
        }
    }

    /// <summary>
    /// Manejador para GetFidsVuelosQuery.
    /// </summary>
    public class GetFidsVuelosHandler : IRequestHandler<GetFidsVuelosQuery, Result<List<DtoFidsVuelo>>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ILogger<GetFidsVuelosHandler> _logger;
        private readonly IValidator<GetFidsVuelosQuery> _validator;

        public GetFidsVuelosHandler(
            IVueloRepository vueloRepository,
            ILogger<GetFidsVuelosHandler> logger,
            IValidator<GetFidsVuelosQuery> validator)
        {
            _vueloRepository = vueloRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<DtoFidsVuelo>>> Handle(GetFidsVuelosQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<DtoFidsVuelo>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                var filtros = new FiltrosFids
                {
                    TipoPantalla = request.TipoPantalla,
                    AeropuertoCodigo = request.AeropuertoCodigo,
                    Estado = request.Estado,
                    AerolineaId = request.AerolineaId,
                    RangoHoras = request.RangoHoras
                };

                var vuelos = await _vueloRepository.BuscarParaFidsAsync(filtros);

                List<DtoFidsVuelo> resultado = new();

                foreach (var vuelo in vuelos)
                {
                    resultado.Add(new DtoFidsVuelo
                    {
                        Id = vuelo.Id,
                        NumeroVuelo = vuelo.NumeroVuelo,
                        AerolineaNombre = vuelo.Aerolinea?.Nombre ?? "N/A",
                        AeropuertoOrigenIATA = vuelo.AeropuertoOrigen?.CodigoIATA ?? "N/A",
                        AeropuertoDestinoIATA = vuelo.AeropuertoDestino?.CodigoIATA ?? "N/A",
                        EstadoActual = vuelo.EstadoActual,
                        PuertaEmbarque = vuelo.PuertaEmbarque,
                        FechaSalidaProgramada = vuelo.SalidaPlanificada,
                        FechaLlegadaProgramada = vuelo.LlegadaPlanificada,
                        SalidaActualizada = vuelo.SalidaActualizada,
                        LlegadaActualizada = vuelo.LlegadaActualizada
                    });
                }

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al consultar los vuelos FIDS.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener los vuelos del tablero.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consultar los vuelos FIDS.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al obtener los vuelos del tablero.";
                return result;
            }
        }
    }
}
