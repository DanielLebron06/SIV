using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Vuelo;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Queries.ObtenerVuelo
{
    /// <summary>
    /// Consulta para obtener los datos de un vuelo por su ID.
    /// </summary>
    public class ObtenerVueloQuery : IRequest<Result<DatosVueloDTO>>
    {
        public Guid VueloId { get; set; }
    }

    /// <summary>
    /// Validador para ObtenerVueloQuery.
    /// </summary>
    public class ObtenerVueloValidator : AbstractValidator<ObtenerVueloQuery>
    {
        public ObtenerVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("Se requiere el ID del vuelo.");
        }
    }

    /// <summary>
    /// Manejador para ObtenerVueloQuery.
    /// </summary>
    public class ObtenerVueloHandler : IRequestHandler<ObtenerVueloQuery, Result<DatosVueloDTO>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ILogger<ObtenerVueloHandler> _logger;
        private readonly IValidator<ObtenerVueloQuery> _validator;

        public ObtenerVueloHandler(
            IVueloRepository vueloRepository,
            ILogger<ObtenerVueloHandler> logger,
            IValidator<ObtenerVueloQuery> validator)
        {
            _vueloRepository = vueloRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<DatosVueloDTO>> Handle(ObtenerVueloQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<DatosVueloDTO>();

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

                var dto = new DatosVueloDTO
                {
                    Id = vuelo.Id,
                    NumeroVuelo = vuelo.NumeroVuelo,
                    AerolineaId = vuelo.AerolineaId,
                    AeropuertoOrigenId = vuelo.AeropuertoOrigenId,
                    AeropuertoDestinoId = vuelo.AeropuertoDestinoId,
                    EstadoActual = vuelo.EstadoActual,
                    FechaSalidaProgramada = vuelo.SalidaPlanificada,
                    FechaLlegadaProgramada = vuelo.LlegadaPlanificada,
                    AerolineaNombre = vuelo.Aerolinea?.Nombre ?? "Sin Aerolínea",
                    AeropuertoOrigenIATA = vuelo.AeropuertoOrigen?.CodigoIATA ?? "N/A",
                    AeropuertoDestinoIATA = vuelo.AeropuertoDestino?.CodigoIATA ?? "N/A"
                };

                result.Success = true;
                result.Data = dto;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener el vuelo.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar el vuelo.";
                return result;
            }
        }
    }
}
