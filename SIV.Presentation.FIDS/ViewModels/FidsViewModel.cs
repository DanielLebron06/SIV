namespace SIV.Presentation.FIDS.ViewModels
{
    public class FidsViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Aeropuerto { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public bool ApiDisponible { get; set; } = true;
        public List<FilaVueloViewModel> Vuelos { get; set; } = new();

        public int PaginaActual { get; set; } = 1;
        public int FilasPorPagina { get; set; } = 10;

        public int TotalPaginas => Vuelos.Count == 0 ? 1 : (int)Math.Ceiling(Vuelos.Count / (double)FilasPorPagina);

        public string UltimaActualizacionTexto => UltimaActualizacion.ToString("HH:mm:ss");
    }
}
