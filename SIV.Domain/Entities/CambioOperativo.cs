using SIV.Domain.Emuns;

namespace SIV.Domain.Entities
{
    public class CambioOperativo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VueloId { get; set; }
        public TipoCambio TipoCambio { get; set; }
        public string Motivo { get; set; } = string.Empty; // Mapeado al payload 'motivo' de SignalR en el SAD
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Nombre exacto del payload del SAD
        public Vuelo? Vuelo { get; set; }
    }
}