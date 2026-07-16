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
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Reportes.Queries.GenerarReporteCambiosOperativos
{
    /// <summary>
    /// Consulta para generar el reporte de cambios operativos.
    /// </summary>
    public class GenerarReporteCambiosOperativosQuery : IRequest<Result<List<ReporteCambioOperativoDTO>>>
    {
        public ReportePeriodoDTO Periodo { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para GenerarReporteCambiosOperativosQuery.
    /// </summary>
    public class GenerarReporteCambiosOperativosValidator : AbstractValidator<GenerarReporteCambiosOperativosQuery>
    {
        public GenerarReporteCambiosOperativosValidator()
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
    /// Manejador para GenerarReporteCambiosOperativosQuery.
    /// </summary>
    public class GenerarReporteCambiosOperativosHandler : IRequestHandler<GenerarReporteCambiosOperativosQuery, Result<List<ReporteCambioOperativoDTO>>>
    {
        private readonly ICambioOperativoRepository _cambioOperativoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GenerarReporteCambiosOperativosHandler> _logger;
        private readonly IValidator<GenerarReporteCambiosOperativosQuery> _validator;

        public GenerarReporteCambiosOperativosHandler(
            ICambioOperativoRepository cambioOperativoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<GenerarReporteCambiosOperativosHandler> logger,
            IValidator<GenerarReporteCambiosOperativosQuery> validator)
        {
            _cambioOperativoRepository = cambioOperativoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<ReporteCambioOperativoDTO>>> Handle(GenerarReporteCambiosOperativosQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<ReporteCambioOperativoDTO>>();

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

                // Uso de ToList() para materialización inmediata.
                var cambios = (await _cambioOperativoRepository.BuscarPorPeriodoAsync(request.Periodo.FechaInicio, request.Periodo.FechaFin)).ToList();

                List<ReporteCambioOperativoDTO> resultado = new();

                foreach (var cambio in cambios)
                {
                    resultado.Add(new ReporteCambioOperativoDTO
                    {
                        Id = cambio.Id,
                        VueloId = cambio.VueloId,
                        NumeroVuelo = cambio.Vuelo != null
                            ? cambio.Vuelo.NumeroVuelo
                            : cambio.VueloId.ToString(),
                        TipoCambio = cambio.TipoCambio,
                        Motivo = cambio.Motivo,
                        FechaCambio = cambio.Timestamp,
                        OperadorId = cambio.UsuarioResponsableId
                    });
                }

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    "Reporte de cambios operativos generado",
                    null,
                    $"{request.Periodo.FechaInicio:yyyy-MM-dd} - {request.Periodo.FechaFin:yyyy-MM-dd}"
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al generar reporte de cambios operativos.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al generar el reporte.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al generar reporte de cambios operativos.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al generar el reporte.";
                return result;
            }
        }
    }
}
