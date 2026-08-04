using System;

namespace SIV.Presentation.Desktop.Vuelos
{
    public class EstadoVueloDTO
    {
        public Guid Id { get; set; }
        public EstadoVuelo Estado { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
