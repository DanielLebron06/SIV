namespace SIV.Presentation.WebUser.ViewModels
{
    public class VueloDetalleViewModel
    {
        public VueloViewModel Vuelo { get; set; } = new();
        public List<HistorialEstadoViewModel> Historial { get; set; } = new();
    }
}
