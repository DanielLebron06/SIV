using System;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class AerolineaDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activa { get; set; }
    }

    public class RegistroAerolineaDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string CodigoIATA { get; set; } = string.Empty;
    }
}
