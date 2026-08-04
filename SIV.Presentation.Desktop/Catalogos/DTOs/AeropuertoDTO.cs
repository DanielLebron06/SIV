using System;

namespace SIV.Presentation.Desktop.Catalogos
{
    public class AeropuertoDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoIATA { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class RegistroAeropuertoDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string CodigoIATA { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
    }
}
