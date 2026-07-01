using SIV.Application.Auditoria;
using SIV.Application.DTOs.Auditoria;
using SIV.Application.DTOs.Reportes;
using SIV.Application.Service.Interfaces;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System.Text;

namespace SIV.Application.Service.Implementations
{
    public class ReportesService : IReportesService
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ICambioOperativoRepository _cambioOperativoRepository;
        private readonly ISeguimientoVueloRepository _seguimientoVueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogAuditoriaRepository _logAuditoriaRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;

        public ReportesService(
            IVueloRepository vueloRepository,
            ICambioOperativoRepository cambioOperativoRepository,
            ISeguimientoVueloRepository seguimientoVueloRepository,
            IUsuarioRepository usuarioRepository,
            ILogAuditoriaRepository logAuditoriaRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork)
        {
            _vueloRepository = vueloRepository;
            _cambioOperativoRepository = cambioOperativoRepository;
            _seguimientoVueloRepository = seguimientoVueloRepository;
            _usuarioRepository = usuarioRepository;
            _logAuditoriaRepository = logAuditoriaRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
        }

        private void ValidarAdministradorOAuditor(Usuario usuario)
        {
            if (usuario.Rol != Rol.Administrador &&
                usuario.Rol != Rol.Auditor)
            {
                throw new Exception("Solo administradores o auditores pueden acceder a reportes");
            }
        }

        private void ValidarPeriodo(ReportePeriodoDTO periodo)
        {
            if (periodo.FechaFin < periodo.FechaInicio)
            {
                throw new Exception("La fecha final no puede ser menor que la fecha inicial");
            }
        }

        private LogAuditoriaDTO MapearLogAuditoria(LogAuditoria log)
        {
            return new LogAuditoriaDTO
            {
                Id = log.Id,
                Actor = log.Actor,
                Modulo = log.Modulo,
                TipoAccion = log.TipoAccion,
                Resultado = log.Resultado,
                EntidadAfectadaId = log.EntidadAfectadaId,
                EntidadAfectadaDescripcion = log.DescripcionEntidad,
                FechaHora = log.FechaHora
            };
        }

        public async Task<ReporteOperacionVuelosDTO> GenerarReporteOperacionVuelos(
            ReportePeriodoDTO periodo,
            Usuario usuario)
        {
            ValidarAdministradorOAuditor(usuario);
            ValidarPeriodo(periodo);

            var vuelos = await _vueloRepository
                .BuscarPorPeriodoAsync(periodo.FechaInicio, periodo.FechaFin);

            var reporte = new ReporteOperacionVuelosDTO
            {
                FechaInicio = periodo.FechaInicio,
                FechaFin = periodo.FechaFin
            };

            foreach (var vuelo in vuelos)
            {
                reporte.TotalVuelosRegistrados++;

                if (vuelo.EstadoActual == EstadoVuelo.Cancelado)
                {
                    reporte.TotalVuelosCancelados++;
                }

                if (vuelo.EstadoActual == EstadoVuelo.Retrasado)
                {
                    reporte.TotalVuelosRetrasados++;
                }

                if (vuelo.EstadoActual == EstadoVuelo.Completado)
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
                $"{periodo.FechaInicio:yyyy-MM-dd} - {periodo.FechaFin:yyyy-MM-dd}"
            );

            await _unitOfWork.SaveChangesAsync();

            return reporte;
        }

        public async Task<List<ReporteCambioOperativoDTO>> GenerarReporteCambiosOperativos(
            ReportePeriodoDTO periodo,
            Usuario usuario)
        {
            ValidarAdministradorOAuditor(usuario);
            ValidarPeriodo(periodo);

            var cambios = await _cambioOperativoRepository
                .BuscarPorPeriodoAsync(periodo.FechaInicio, periodo.FechaFin);

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
                $"{periodo.FechaInicio:yyyy-MM-dd} - {periodo.FechaFin:yyyy-MM-dd}"
            );

            await _unitOfWork.SaveChangesAsync();

            return resultado;
        }

        public async Task<ReporteSeguimientoDTO> GenerarReporteSeguimiento(
            ReportePeriodoDTO periodo,
            Usuario usuario)
        {
            ValidarAdministradorOAuditor(usuario);
            ValidarPeriodo(periodo);

            var seguimientos = await _seguimientoVueloRepository
                .BuscarPorPeriodoAsync(periodo.FechaInicio, periodo.FechaFin);

            var reporte = new ReporteSeguimientoDTO
            {
                FechaInicio = periodo.FechaInicio,
                FechaFin = periodo.FechaFin
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
                $"{periodo.FechaInicio:yyyy-MM-dd} - {periodo.FechaFin:yyyy-MM-dd}"
            );

            await _unitOfWork.SaveChangesAsync();

            return reporte;
        }

        public async Task<List<LogAuditoriaDTO>> ConsultarLogAuditoria(
            FiltroAuditoriaDTO filtros,
            Usuario usuario)
        {
            ValidarAdministradorOAuditor(usuario);

            var logs = await _logAuditoriaRepository.BuscarConFiltrosAsync(
                filtros.Actor,
                filtros.Modulo,
                filtros.TipoAccion,
                filtros.FechaInicio,
                filtros.FechaFin
            );

            List<LogAuditoriaDTO> resultado = new();

            foreach (var log in logs)
            {
                resultado.Add(MapearLogAuditoria(log));
            }

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Actualizar,
                "Consulta de log de auditoría realizada",
                null,
                usuario.Email
            );

            await _unitOfWork.SaveChangesAsync();

            return resultado;
        }

        public async Task<string> ExportarReporteOperacionVuelosCsv(
            ReportePeriodoDTO periodo,
            Usuario usuario)
        {
            var reporte = await GenerarReporteOperacionVuelos(periodo, usuario);

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

            return csv.ToString();
        }
    }
}