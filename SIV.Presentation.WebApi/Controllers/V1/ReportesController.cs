using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SIV.Application.Service.Interfaces;
using SIV.Application.DTOs.Reportes;
using SIV.Application.DTOs.Auditoria;
using SIV.Domain.Entities;
using System.Text;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Administrador, Auditor")]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;
        private readonly IUserService _userService;

        public ReportesController(IReportesService reportesService, IUserService userService)
        {
            _reportesService = reportesService;
            _userService = userService;
        }

        // GET /api/v1/Reportes/operacion-vuelos
        [HttpGet("operacion-vuelos")]
        public async Task<IActionResult> GetOperacionVuelos([FromQuery] ReportePeriodoDTO periodo)
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity.Name);
            var reporte = await _reportesService.GenerarReporteOperacionVuelos(periodo, usuarioEjecutador);
            return Ok(reporte);
        }

        // GET /api/v1/Reportes/operacion-vuelos/csv
        [HttpGet("operacion-vuelos/csv")]
        public async Task<IActionResult> GetOperacionVuelosCsv([FromQuery] ReportePeriodoDTO periodo)
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity.Name);
            var csv = await _reportesService.ExportarReporteOperacionVuelosCsv(periodo, usuarioEjecutador);
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "operacion-vuelos.csv");
        }

        // GET /api/v1/Reportes/auditoria
        [HttpGet("auditoria")]
        public async Task<IActionResult> GetAuditoria([FromQuery] FiltroAuditoriaDTO filtros)
        {
            var usuarioEjecutador = await _userService.ObtenerPorEmail(User.Identity.Name);
            var auditoria = await _reportesService.ConsultarLogAuditoria(filtros, usuarioEjecutador);
            return Ok(auditoria);
        }
    }
}
