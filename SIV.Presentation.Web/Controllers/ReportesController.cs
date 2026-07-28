using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Auditoria;
using SIV.Application.DTOs.Reportes;
using SIV.Application.Features.Reportes.Queries.ConsultarLogAuditoria;
using SIV.Application.Features.Reportes.Queries.ExportarReporteOperacionVuelosCsv;
using SIV.Application.Features.Reportes.Queries.GenerarReporteCambiosOperativos;
using SIV.Application.Features.Reportes.Queries.GenerarReporteOperacionVuelos;
using SIV.Application.Features.Reportes.Queries.GenerarReporteSeguimiento;
using System.Security.Claims;
using System.Text;

namespace SIV.Presentation.Web.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ISender _sender;


        private static readonly Guid UsuarioPruebasId =
            Guid.Parse("352717f7-14e6-4dfe-a183-aeaa21717ae3");


        public ReportesController(ISender sender)
        {
            _sender = sender;
        }

        private Guid ObtenerUsuarioActual()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return UsuarioPruebasId;

            return Guid.Parse(userId);
        }

        // GET: Reportes
        public IActionResult Index()
        {
            return View();
        }


        // GET: Reportes/OperacionVuelos
        public async Task<IActionResult> OperacionVuelos(
            ReportePeriodoDTO periodo)
        {

            var result = await _sender.Send(
                new GenerarReporteOperacionVuelosQuery
                {
                    Periodo = periodo,
                    EjecutadorId = ObtenerUsuarioActual()
                });


            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new ReporteOperacionVuelosDTO());
            }


            return View(result.Data);
        }

        // Descargar CSV
        public async Task<IActionResult> ExportarOperacionVuelosCsv(
            ReportePeriodoDTO periodo)
        {

            var result = await _sender.Send(
                new ExportarReporteOperacionVuelosCsvQuery
                {
                    Periodo = periodo,
                    EjecutadorId = ObtenerUsuarioActual()
                });


            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(OperacionVuelos));
            }


            return File(
                Encoding.UTF8.GetBytes(result.Data),
                "text/csv",
                "operacion-vuelos.csv");
        }

        // GET: Reportes/Auditoria
        public async Task<IActionResult> Auditoria(
            FiltroAuditoriaDTO filtros)
        {

            var result = await _sender.Send(
                new ConsultarLogAuditoriaQuery
                {
                    Filtros = filtros,
                    EjecutadorId = ObtenerUsuarioActual()
                });


            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return View(
                    Enumerable.Empty<LogAuditoriaDTO>());
            }


            return View(result.Data);
        }


        // GET: Reportes/CambiosOperativos
        public async Task<IActionResult> CambiosOperativos(
            ReportePeriodoDTO periodo)
        {

            var result = await _sender.Send(
                new GenerarReporteCambiosOperativosQuery
                {
                    Periodo = periodo,
                    EjecutadorId = ObtenerUsuarioActual()
                });


            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return View(
                    Enumerable.Empty<ReporteCambioOperativoDTO>());
            }


            return View(result.Data);
        }


        // GET: Reportes/Seguimiento
        public async Task<IActionResult> Seguimiento(
            ReportePeriodoDTO periodo)
        {
            if (periodo.FechaInicio == default)
            {
                periodo.FechaInicio = DateTime.Today.AddDays(-30);
            }
            if (periodo.FechaFin == default)
            {
                periodo.FechaFin = DateTime.Now;
            }

            var result = await _sender.Send(
                new GenerarReporteSeguimientoQuery
                {
                    Periodo = periodo,
                    EjecutadorId = ObtenerUsuarioActual()
                });


            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return View(
                    new ReporteSeguimientoDTO
                    {
                        FechaInicio = periodo.FechaInicio,
                        FechaFin = periodo.FechaFin
                    });
            }


            return View(result.Data);
        }

    }
}