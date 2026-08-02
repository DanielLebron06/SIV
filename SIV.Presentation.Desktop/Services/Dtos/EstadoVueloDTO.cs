using System;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class EstadoVueloDTO
    {
        public Guid Id { get; set; }
        public EstadoVuelo Estado { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
