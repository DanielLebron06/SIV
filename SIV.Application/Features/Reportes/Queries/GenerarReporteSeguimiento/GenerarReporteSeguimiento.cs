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

namespace SIV.Application.Features.Reportes.Queries.GenerarReporteSeguimiento
{
    /// <summary>
    /// Consulta para generar el reporte de seguimientos de vuelos.
    /// </summary>
    public class GenerarReporteSeguimientoQuery : IRequest<Result<ReporteSeguimientoDTO>>
    {
        public ReportePeriodoDTO Periodo { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para GenerarReporteSeguimientoQuery.
    /// </summary>
    public class GenerarReporteSeguimientoValidator : AbstractValidator<GenerarReporteSeguimientoQuery>
    {
        public GenerarReporteSeguimientoValidator()
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
    /// Manejador para GenerarReporteSeguimientoQuery.
    /// </summary>
    public class GenerarReporteSeguimientoHandler : IRequestHandler<GenerarReporteSeguimientoQuery, Result<ReporteSeguimientoDTO>>
    {
        private readonly ISeguimientoVueloRepository _seguimientoVueloRepository;
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GenerarReporteSeguimientoHandler> _logger;
        private readonly IValidator<GenerarReporteSeguimientoQuery> _validator;

        public GenerarReporteSeguimientoHandler(
            ISeguimientoVueloRepository seguimientoVueloRepository,
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<GenerarReporteSeguimientoHandler> logger,
            IValidator<GenerarReporteSeguimientoQuery> validator)
        {
            _seguimientoVueloRepository = seguimientoVueloRepository;
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<ReporteSeguimientoDTO>> Handle(GenerarReporteSeguimientoQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<ReporteSeguimientoDTO>();

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

                // Uso de ToList() para evitar deferred execution.
                var seguimientos = (await _seguimientoVueloRepository.BuscarPorPeriodoAsync(request.Periodo.FechaInicio, request.Periodo.FechaFin)).ToList();

                var reporte = new ReporteSeguimientoDTO
                {
                    FechaInicio = request.Periodo.FechaInicio,
                    FechaFin = request.Periodo.FechaFin
                };

                foreach (var seguimiento in seguimientos)
                {
                    reporte.TotalSeguimientosIniciados++;

                    if (seguimiento.FechaFin != null)
                    {
                        reporte.TotalSeguimientosFinalizados++;
                    }
                }

                var agrupadosPorVuelo = seguimientos
                    .GroupBy(s => s.VueloId)
                    .OrderByDescending(g => g.Count())
                    .Take(10)
                    .ToList();

                foreach (var grupo in agrupadosPorVuelo)
                {
                    var vuelo = await _vueloRepository.GetByIdAsync(grupo.Key);

                    reporte.VuelosMasSeguidos.Add(new VueloMasSeguidoDTO
                    {
                        VueloId = grupo.Key,
                        NumeroVuelo = vuelo != null ? vuelo.NumeroVuelo : grupo.Key.ToString(),
                        TotalSeguidores = grupo.Count()
                    });
                }

                var agrupadosPorUsuario = seguimientos
                    .GroupBy(s => s.UsuarioId)
                    .OrderByDescending(g => g.Count())
                    .Take(10)
                    .ToList();

                foreach (var grupo in agrupadosPorUsuario)
                {
                    var usuarioReporte = await _usuarioRepository.GetByIdAsync(grupo.Key);

                    reporte.UsuariosActivos.Add(new UsuarioActivoSeguimientoDTO
                    {
                        UsuarioId = grupo.Key,
                        Email = usuarioReporte != null ? usuarioReporte.Email : grupo.Key.ToString(),
                        TotalSeguimientos = grupo.Count()
                    });
                }

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    "Reporte de seguimiento generado",
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
                _logger.LogError(ex, "Error de base de datos al generar reporte de seguimiento.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al generar el reporte.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al generar reporte de seguimiento.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al generar el reporte.";
                return result;
            }
        }
    }
}
