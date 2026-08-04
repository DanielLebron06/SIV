namespace SIV.Presentation.WebUser.ViewModels.Vuelos
{
    public class VueloDetalleViewModel
    {
        public VueloViewModel Vuelo { get; set; } = new();
        public List<HistorialEstadoViewModel> Historial { get; set; } = new();
        public bool EstaSiguiendo { get; set; }
    }
}
