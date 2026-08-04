using System;

namespace SIV.Presentation.Desktop.Vuelos
{
    public class CambioOperativoDTO
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public TipoCambio TipoCambio { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Guid UsuarioResponsableId { get; set; }
    }
}
