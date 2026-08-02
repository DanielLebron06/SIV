using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIV.Presentation.WebUser.ViewModels
{
    public class FiltrosVuelosViewModel
    {
        public Guid? AerolineaId { get; set; }
        public Guid? AeropuertoOrigenId { get; set; }
        public Guid? AeropuertoDestinoId { get; set; }
        public DateTimeOffset? Fecha { get; set; }
        public EstadoVuelo? Estado { get; set; }

        public List<AerolineaViewModel> Aerolineas { get; set; } = new();
        public List<AeropuertoViewModel> Aeropuertos { get; set; } = new();
        public List<VueloViewModel> Vuelos { get; set; } = new();

        public IEnumerable<SelectListItem> OpcionesAerolineas =>
            Aerolineas.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre });

        public IEnumerable<SelectListItem> OpcionesAeropuertos =>
            Aeropuertos.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.CodigoIATA} - {a.Nombre}" });

        public IEnumerable<SelectListItem> OpcionesEstados =>
            Enum.GetValues<EstadoVuelo>().Select(e => new SelectListItem { Value = e.ToString(), Text = DescripcionEstado(e) });

        public static string DescripcionEstado(EstadoVuelo estado) => estado switch
        {
            EstadoVuelo.Programado => "Programado",
            EstadoVuelo.Retrasado => "Retrasado",
            EstadoVuelo.Embarcando => "Embarcando",
            EstadoVuelo.EnVuelo => "En vuelo",
            EstadoVuelo.Aterrizado => "Aterrizado",
            EstadoVuelo.Completado => "Completado",
            EstadoVuelo.Cancelado => "Cancelado",
            _ => estado.ToString()
        };
    }
}
