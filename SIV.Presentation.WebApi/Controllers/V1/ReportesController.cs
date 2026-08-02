using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Auditoria;
using SIV.Application.DTOs.Reportes;
using SIV.Application.Features.Reportes.Queries.ConsultarLogAuditoria;
using SIV.Application.Features.Reportes.Queries.ExportarReporteOperacionVuelosCsv;
using SIV.Application.Features.Reportes.Queries.GenerarReporteCambiosOperativos;
using SIV.Application.Features.Reportes.Queries.GenerarReporteOperacionVuelos;
using SIV.Application.Features.Reportes.Queries.GenerarReporteSeguimiento;
using SIV.Presentation.WebApi.Common;
using System.Security.Claims;
using System.Text;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Administrador, Auditor")]
    [Produces("application/json")]
    public class ReportesController : ControllerBase
    {
        private readonly ISender _sender;

        public ReportesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("operacion-vuelos")]
        [ProducesResponseType(typeof(ReporteOperacionVuelosDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOperacionVuelos([FromQuery] ReportePeriodoDTO periodo)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new GenerarReporteOperacionVuelosQuery { Periodo = periodo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpGet("operacion-vuelos/csv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOperacionVuelosCsv([FromQuery] ReportePeriodoDTO periodo)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ExportarReporteOperacionVuelosCsvQuery { Periodo = periodo, EjecutadorId = ejecutadorId });
            if (!result.Success || result.Data == null) return BadRequest(ApiResponse.Error(result.Message));
            return File(Encoding.UTF8.GetBytes(result.Data), "text/csv", "operacion-vuelos.csv");
        }

        [HttpGet("auditoria")]
        [ProducesResponseType(typeof(List<LogAuditoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAuditoria([FromQuery] FiltroAuditoriaDTO filtros)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new ConsultarLogAuditoriaQuery { Filtros = filtros, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpGet("cambios-operativos")]
        [ProducesResponseType(typeof(List<ReporteCambioOperativoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCambiosOperativos([FromQuery] ReportePeriodoDTO periodo)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new GenerarReporteCambiosOperativosQuery { Periodo = periodo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }

        [HttpGet("seguimiento")]
        [ProducesResponseType(typeof(ReporteSeguimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetSeguimiento([FromQuery] ReportePeriodoDTO periodo)
        {
            var ejecutadorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            var result = await _sender.Send(new GenerarReporteSeguimientoQuery { Periodo = periodo, EjecutadorId = ejecutadorId });
            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }
    }
}
