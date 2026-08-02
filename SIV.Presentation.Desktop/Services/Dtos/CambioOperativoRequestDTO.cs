using System;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class CambioOperativoTiempoDTO
    {
        public DateTimeOffset NuevaHoraEstimada { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    public class CambioPuertaDTO
    {
        public string NuevaPuerta { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
    }

    public class CancelarVueloDTO
    {
        public string Motivo { get; set; } = string.Empty;
    }

    public class ActualizarEstadoDTO
    {
        public EstadoVuelo NuevoEstado { get; set; }
    }
}
