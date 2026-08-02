namespace SIV.Presentation.FIDS.ViewModels
{
    public class FilaVueloViewModel
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string AerolineaNombre { get; set; } = string.Empty;
        public string OrigenIATA { get; set; } = string.Empty;
        public string DestinoIATA { get; set; } = string.Empty;
        public DateTimeOffset HoraProgramada { get; set; }
        public DateTimeOffset? HoraEstimada { get; set; }
        public string PuertaEmbarque { get; set; } = string.Empty;
        public EstadoFids Estado { get; set; }

        public string HoraProgramadaTexto => HoraProgramada.ToLocalTime().ToString("HH:mm");
        public string HoraEstimadaTexto => HoraEstimada?.ToLocalTime().ToString("HH:mm") ?? "--:--";
        public string EstadoTexto => Estado switch
        {
            EstadoFids.Programado => "PROGRAMADO",
            EstadoFids.Embarcando => "EMBARCANDO",
            EstadoFids.UltimoLlamado => "ÚLTIMO LLAMADO",
            EstadoFids.EnHora => "EN HORA",
            EstadoFids.Retrasado => "RETRASADO",
            EstadoFids.Cancelado => "CANCELADO",
            _ => "DESCONOCIDO"
        };
        public string BadgeClass => Estado switch
        {
            EstadoFids.Programado => "badge-estado badge-programado",
            EstadoFids.Embarcando => "badge-estado badge-embarcando",
            EstadoFids.UltimoLlamado => "badge-estado badge-ultimo-llamado",
            EstadoFids.EnHora => "badge-estado badge-en-hora",
            EstadoFids.Retrasado => "badge-estado badge-retrasado",
            EstadoFids.Cancelado => "badge-estado badge-cancelado",
            _ => "badge-estado badge-desconocido"
        };
    }
}
