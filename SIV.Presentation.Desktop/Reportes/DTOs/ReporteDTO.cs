using SIV.Presentation.Desktop.Vuelos;
using System;
using System.Collections.Generic;

namespace SIV.Presentation.Desktop.Reportes
{
    public class ReportePeriodoDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class ReporteOperacionVuelosDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalVuelosRegistrados { get; set; }
        public int TotalVuelosCancelados { get; set; }
        public int TotalVuelosRetrasados { get; set; }
        public int TotalVuelosCompletados { get; set; }
    }

    public class ReporteCambioOperativoDTO
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public TipoCambio TipoCambio { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; }
        public Guid OperadorId { get; set; }
    }

    public class ReporteSeguimientoDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalSeguimientosIniciados { get; set; }
        public int TotalSeguimientosFinalizados { get; set; }
        public List<VueloMasSeguidoDTO> VuelosMasSeguidos { get; set; } = new List<VueloMasSeguidoDTO>();
        public List<UsuarioActivoSeguimientoDTO> UsuariosActivos { get; set; } = new List<UsuarioActivoSeguimientoDTO>();
    }

    public class VueloMasSeguidoDTO
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public int TotalSeguidores { get; set; }
    }

    public class UsuarioActivoSeguimientoDTO
    {
        public Guid UsuarioId { get; set; }
        public string Email { get; set; } = string.Empty;
        public int TotalSeguimientos { get; set; }
    }
}
