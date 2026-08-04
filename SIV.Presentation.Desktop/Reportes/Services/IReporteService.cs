using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Reportes
{
    public interface IReporteService
    {
        Task<ReporteOperacionVuelosDTO> ObtenerReporteOperacionVuelosAsync(ReportePeriodoDTO periodo);
        Task<byte[]> ExportarReporteOperacionVuelosCsvAsync(ReportePeriodoDTO periodo);
        Task<List<LogAuditoriaDTO>> ObtenerLogAuditoriaAsync(FiltroAuditoriaDTO filtros);
        Task<List<ReporteCambioOperativoDTO>> ObtenerReporteCambiosOperativosAsync(ReportePeriodoDTO periodo);
        Task<ReporteSeguimientoDTO> ObtenerReporteSeguimientoAsync(ReportePeriodoDTO periodo);
    }
}
