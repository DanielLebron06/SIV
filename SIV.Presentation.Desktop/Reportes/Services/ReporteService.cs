using SIV.Presentation.Desktop.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Reportes
{
    public class ReporteService : IReporteService
    {
        private readonly ApiClient _apiClient;

        public ReporteService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ReporteOperacionVuelosDTO> ObtenerReporteOperacionVuelosAsync(ReportePeriodoDTO periodo)
        {
            var query = QueryBuilder.Build(new Dictionary<string, object>
            {
                { "FechaInicio", periodo.FechaInicio },
                { "FechaFin", periodo.FechaFin }
            });

            return await _apiClient.GetAsync<ReporteOperacionVuelosDTO>("Reportes/operacion-vuelos?" + query);
        }

        public async Task<byte[]> ExportarReporteOperacionVuelosCsvAsync(ReportePeriodoDTO periodo)
        {
            var query = QueryBuilder.Build(new Dictionary<string, object>
            {
                { "FechaInicio", periodo.FechaInicio },
                { "FechaFin", periodo.FechaFin }
            });

            return await _apiClient.GetBytesAsync("Reportes/operacion-vuelos/csv?" + query);
        }

        public async Task<List<LogAuditoriaDTO>> ObtenerLogAuditoriaAsync(FiltroAuditoriaDTO filtros)
        {
            var query = QueryBuilder.Build(new Dictionary<string, object>
            {
                { "Actor", filtros?.Actor },
                { "Modulo", filtros?.Modulo },
                { "TipoAccion", filtros?.TipoAccion },
                { "FechaInicio", filtros?.FechaInicio },
                { "FechaFin", filtros?.FechaFin }
            });

            return await _apiClient.GetAsync<List<LogAuditoriaDTO>>("Reportes/auditoria?" + query);
        }

        public async Task<List<ReporteCambioOperativoDTO>> ObtenerReporteCambiosOperativosAsync(ReportePeriodoDTO periodo)
        {
            var query = QueryBuilder.Build(new Dictionary<string, object>
            {
                { "FechaInicio", periodo.FechaInicio },
                { "FechaFin", periodo.FechaFin }
            });

            return await _apiClient.GetAsync<List<ReporteCambioOperativoDTO>>("Reportes/cambios-operativos?" + query);
        }

        public async Task<ReporteSeguimientoDTO> ObtenerReporteSeguimientoAsync(ReportePeriodoDTO periodo)
        {
            var query = QueryBuilder.Build(new Dictionary<string, object>
            {
                { "FechaInicio", periodo.FechaInicio },
                { "FechaFin", periodo.FechaFin }
            });

            return await _apiClient.GetAsync<ReporteSeguimientoDTO>("Reportes/seguimiento?" + query);
        }
    }
}
