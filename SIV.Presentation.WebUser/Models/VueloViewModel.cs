namespace SIV.Presentation.WebUser.Models
{
    public class VueloViewModel
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string AerolineaNombre { get; set; } = string.Empty;
        public string AeropuertoOrigenIATA { get; set; } = string.Empty;
        public string AeropuertoDestinoIATA { get; set; } = string.Empty;
        public DateTime FechaSalidaProgramada { get; set; }
        public int EstadoActual { get; set; }
        public string EstadoTexto => EstadoActual switch
        {
            0 => "Programado",
            1 => "Retrasado",
            2 => "Embarcando",
            3 => "En Vuelo",
            4 => "Aterrizado",
            5 => "Completado",
            6 => "Cancelado"
        };
    }
}
