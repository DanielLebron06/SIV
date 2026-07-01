using SIV.Application.DTOs.Auditoria;
using SIV.Application.DTOs.Reportes;
using SIV.Domain.Entities;

namespace SIV.Application.Service.Interfaces
{
    public interface IReportesService
    {
        Task<ReporteOperacionVuelosDTO> GenerarReporteOperacionVuelos(
            ReportePeriodoDTO periodo,
            Usuario usuario);

        Task<List<ReporteCambioOperativoDTO>> GenerarReporteCambiosOperativos(
            ReportePeriodoDTO periodo,
            Usuario usuario);

        Task<ReporteSeguimientoDTO> GenerarReporteSeguimiento(
            ReportePeriodoDTO periodo,
            Usuario usuario);

        Task<List<LogAuditoriaDTO>> ConsultarLogAuditoria(
            FiltroAuditoriaDTO filtros,
            Usuario usuario);

        Task<string> ExportarReporteOperacionVuelosCsv(
            ReportePeriodoDTO periodo,
            Usuario usuario);
    }
}