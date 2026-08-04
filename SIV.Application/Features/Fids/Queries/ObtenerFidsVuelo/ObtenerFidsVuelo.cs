using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Fids;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Fids.Queries.ObtenerFidsVuelo
{
    /// <summary>
    /// Consulta para obtener un vuelo FIDS por su ID.
    /// </summary>
    public class ObtenerFidsVueloQuery : IRequest<Result<DtoFidsVuelo>>
    {
        public Guid VueloId { get; set; }
    }

    /// <summary>
    /// Validador para ObtenerFidsVueloQuery.
    /// </summary>
    public class ObtenerFidsVueloValidator : AbstractValidator<ObtenerFidsVueloQuery>
    {
        public ObtenerFidsVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("Se requiere el ID del vuelo.");
        }
    }

    /// <summary>
    /// Manejador para ObtenerFidsVueloQuery.
    /// </summary>
    public class ObtenerFidsVueloHandler : IRequestHandler<ObtenerFidsVueloQuery, Result<DtoFidsVuelo>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ILogger<ObtenerFidsVueloHandler> _logger;
        private readonly IValidator<ObtenerFidsVueloQuery> _validator;

        public ObtenerFidsVueloHandler(
            IVueloRepository vueloRepository,
            ILogger<ObtenerFidsVueloHandler> logger,
            IValidator<ObtenerFidsVueloQuery> validator)
        {
            _vueloRepository = vueloRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<DtoFidsVuelo>> Handle(ObtenerFidsVueloQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<DtoFidsVuelo>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                var vuelo = await _vueloRepository.GetVueloConDetallesAsync(request.VueloId);

                if (vuelo == null)
                {
                    result.Success = false;
                    result.Message = "Vuelo no encontrado.";
                    return result;
                }

                var dto = new DtoFidsVuelo
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
                };

                result.Success = true;
                result.Data = dto;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener el vuelo FIDS.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener el vuelo del tablero.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el vuelo FIDS.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar el vuelo del tablero.";
                return result;
            }
        }
    }
}
