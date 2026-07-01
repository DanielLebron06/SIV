

namespace SIV.Application.DTOs.Aeropuerto
{
    public class AeropuertoDTO
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string CodigoIATA { get; set; }

        public bool Activo { get; set; }
    }
}