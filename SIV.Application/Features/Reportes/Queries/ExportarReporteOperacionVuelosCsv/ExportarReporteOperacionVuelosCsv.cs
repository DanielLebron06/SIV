using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Reportes;
using SIV.Application.Features.Reportes.Queries.GenerarReporteOperacionVuelos;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Reportes.Queries.ExportarReporteOperacionVuelosCsv
{
    /// <summary>
    /// Consulta para exportar a CSV el reporte de operación de vuelos.
    /// Retorna el CSV en formato string.
    /// </summary>
    public class ExportarReporteOperacionVuelosCsvQuery : IRequest<Result<string>>
    {
        public ReportePeriodoDTO Periodo { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para ExportarReporteOperacionVuelosCsvQuery.
    /// </summary>
    public class ExportarReporteOperacionVuelosCsvValidator : AbstractValidator<ExportarReporteOperacionVuelosCsvQuery>
    {
        public ExportarReporteOperacionVuelosCsvValidator()
        {
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
            RuleFor(x => x.Periodo).NotNull().WithMessage("El periodo del reporte es requerido.");
        }
    }

    /// <summary>
    /// Manejador para ExportarReporteOperacionVuelosCsvQuery.
    /// Utiliza el Mediator inyectado para re-utilizar la lógica de GenerarReporteOperacionVuelosQuery.
    /// </summary>
    public class ExportarReporteOperacionVuelosCsvHandler : IRequestHandler<ExportarReporteOperacionVuelosCsvQuery, Result<string>>
    {
        private readonly ISender _mediator;
        private readonly ILogger<ExportarReporteOperacionVuelosCsvHandler> _logger;
        private readonly IValidator<ExportarReporteOperacionVuelosCsvQuery> _validator;

        public ExportarReporteOperacionVuelosCsvHandler(
            ISender mediator,
            ILogger<ExportarReporteOperacionVuelosCsvHandler> logger,
            IValidator<ExportarReporteOperacionVuelosCsvQuery> validator)
        {
            _mediator = mediator;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<string>> Handle(ExportarReporteOperacionVuelosCsvQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<string>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Reutilizamos el query existente para obtener los datos
                var reporteQuery = new GenerarReporteOperacionVuelosQuery
                {
                    EjecutadorId = request.EjecutadorId,
                    Periodo = request.Periodo
                };

                var reporteResult = await _mediator.Send(reporteQuery, cancellationToken);

                if (!reporteResult.Success || reporteResult.Data == null)
                {
                    result.Success = false;
                    result.Message = reporteResult.Message ?? "No se pudo obtener la información para el reporte.";
                    return result;
                }

                var reporte = reporteResult.Data;

                StringBuilder csv = new();
                csv.AppendLine("FechaInicio,FechaFin,Registrados,Cancelados,Retrasados,Completados");

                csv.AppendLine(
                    $"{reporte.FechaInicio:yyyy-MM-dd}," +
                    $"{reporte.FechaFin:yyyy-MM-dd}," +
                    $"{reporte.TotalVuelosRegistrados}," +
                    $"{reporte.TotalVuelosCancelados}," +
                    $"{reporte.TotalVuelosRetrasados}," +
                    $"{reporte.TotalVuelosCompletados}"
                );

                result.Success = true;
                result.Data = csv.ToString();
                return result;
            }
            // No capturamos DbException aquí porque el llamado a la base de datos lo hace el otro handler.
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al exportar el reporte a CSV.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al generar el archivo CSV.";
                return result;
            }
        }
    }
}
