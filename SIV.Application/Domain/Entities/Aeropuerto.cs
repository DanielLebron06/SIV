namespace SIV.Application.Domain.Entities
{
    public class Aeropuerto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty; // Ej: "Aeropuerto Int. Las Américas"
        public string CodigoIATA { get; set; } = string.Empty; // Ej: "SDQ"
        public string Ciudad { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
    }
}
