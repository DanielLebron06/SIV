using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Reportes;
using SIV.Domain.Emuns;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Reportes.Queries.GenerarReporteOperacionVuelos
{
    /// <summary>
    /// Consulta para generar el reporte de operación de vuelos.
    /// </summary>
    public class GenerarReporteOperacionVuelosQuery : IRequest<Result<ReporteOperacionVuelosDTO>>
    {
        public ReportePeriodoDTO Periodo { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para GenerarReporteOperacionVuelosQuery.
    /// </summary>
    public class GenerarReporteOperacionVuelosValidator : AbstractValidator<GenerarReporteOperacionVuelosQuery>
    {
        public GenerarReporteOperacionVuelosValidator()
        {
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
            RuleFor(x => x.Periodo).NotNull().WithMessage("El periodo del reporte es requerido.");
            RuleFor(x => x.Periodo.FechaFin)
                .GreaterThanOrEqualTo(x => x.Periodo.FechaInicio)
                .When(x => x.Periodo != null)
                .WithMessage("La fecha final no puede ser menor que la fecha inicial.");
        }
    }

    /// <summary>
    /// Manejador para GenerarReporteOperacionVuelosQuery.
    /// </summary>
    public class GenerarReporteOperacionVuelosHandler : IRequestHandler<GenerarReporteOperacionVuelosQuery, Result<ReporteOperacionVuelosDTO>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GenerarReporteOperacionVuelosHandler> _logger;
        private readonly IValidator<GenerarReporteOperacionVuelosQuery> _validator;

        public GenerarReporteOperacionVuelosHandler(
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<GenerarReporteOperacionVuelosHandler> logger,
            IValidator<GenerarReporteOperacionVuelosQuery> validator)
        {
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<ReporteOperacionVuelosDTO>> Handle(GenerarReporteOperacionVuelosQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<ReporteOperacionVuelosDTO>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                var usuario = await _usuarioRepository.GetByIdAsync(request.EjecutadorId);
                if (usuario == null || !usuario.EsAuditorOAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo administradores o auditores pueden acceder a reportes.";
                    return result;
                }

                // Uso de ToList() para materializar inmediatamente
                var vuelos = (await _vueloRepository.BuscarPorPeriodoAsync(request.Periodo.FechaInicio, request.Periodo.FechaFin)).ToList();

                var reporte = new ReporteOperacionVuelosDTO
                {
                    FechaInicio = request.Periodo.FechaInicio,
                    FechaFin = request.Periodo.FechaFin
                };

                foreach (var vuelo in vuelos)
                {
                    reporte.TotalVuelosRegistrados++;

                    if (vuelo.EstadoActual == EstadoVuelo.Cancelado)
                    {
                        reporte.TotalVuelosCancelados++;
                    }
                    else if (vuelo.EstadoActual == EstadoVuelo.Retrasado)
                    {
                        reporte.TotalVuelosRetrasados++;
                    }
                    else if (vuelo.EstadoActual == EstadoVuelo.Completado)
                    {
                        reporte.TotalVuelosCompletados++;
                    }
                }

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    "Reporte de operación de vuelos generado",
                    null,
                    $"{request.Periodo.FechaInicio:yyyy-MM-dd} - {request.Periodo.FechaFin:yyyy-MM-dd}"
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = reporte;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al generar reporte de operación de vuelos.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al generar el reporte.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al generar reporte de operación de vuelos.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al generar el reporte.";
                return result;
            }
        }
    }
}
