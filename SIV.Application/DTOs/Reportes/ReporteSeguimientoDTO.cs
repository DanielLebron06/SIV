namespace SIV.Application.DTOs.Reportes
{
    public class ReporteSeguimientoDTO
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int TotalSeguimientosIniciados { get; set; }

        public int TotalSeguimientosFinalizados { get; set; }

        public List<VueloMasSeguidoDTO> VuelosMasSeguidos { get; set; } = new();

        public List<UsuarioActivoSeguimientoDTO> UsuariosActivos { get; set; } = new();
    }
}