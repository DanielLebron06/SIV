namespace SIV.Application.Domain.Entities
{
    public class Aerolinea
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty; // Ej: "Delta Air Lines"
        public string CodigoIATA { get; set; } = string.Empty; // Ej: "DL" (Obligatorio en aeronáutica)
    }
}
