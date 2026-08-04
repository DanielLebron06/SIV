namespace SIV.Presentation.FIDS.Services.Dtos
{
    public class VueloFidsDto
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string AerolineaNombre { get; set; } = string.Empty;
        public string AeropuertoOrigenIATA { get; set; } = string.Empty;
        public string AeropuertoDestinoIATA { get; set; } = string.Empty;
        public int EstadoActual { get; set; }
        public string? PuertaEmbarque { get; set; }
        public string? BandaEquipaje { get; set; }
        public string? Terminal { get; set; }
        public DateTimeOffset FechaSalidaProgramada { get; set; }
        public DateTimeOffset FechaLlegadaProgramada { get; set; }
        public DateTimeOffset? SalidaActualizada { get; set; }
        public DateTimeOffset? LlegadaActualizada { get; set; }
    }
}
