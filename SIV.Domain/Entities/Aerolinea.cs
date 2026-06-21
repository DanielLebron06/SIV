namespace SIV.Domain.Entities
{
    public class Aerolinea
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string CodigoIATA { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}