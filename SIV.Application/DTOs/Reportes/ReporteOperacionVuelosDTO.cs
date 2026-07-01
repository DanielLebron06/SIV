namespace SIV.Application.DTOs.Reportes
{
    public class ReporteOperacionVuelosDTO
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int TotalVuelosRegistrados { get; set; }

        public int TotalVuelosCancelados { get; set; }

        public int TotalVuelosRetrasados { get; set; }

        public int TotalVuelosCompletados { get; set; }
    }
}