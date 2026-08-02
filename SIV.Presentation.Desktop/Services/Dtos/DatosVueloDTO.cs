using System;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class DatosVueloDTO
    {
        public string NumeroVuelo { get; set; } = string.Empty;
        public Guid AerolineaId { get; set; }
        public Guid AeropuertoOrigenId { get; set; }
        public Guid AeropuertoDestinoId { get; set; }
        public DateTimeOffset FechaSalidaProgramada { get; set; }
        public DateTimeOffset FechaLlegadaProgramada { get; set; }
    }
}
