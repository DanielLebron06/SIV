using SIV.Presentation.FIDS.Services.Dtos;
using SIV.Presentation.FIDS.ViewModels;

namespace SIV.Presentation.FIDS.Services
{
    public static class VueloMapper
    {
        public static List<FilaVueloViewModel> Mapear(List<VueloFidsDto> vuelos, bool esSalida, DateTimeOffset ahora)
        {
            return vuelos.Select(v => new FilaVueloViewModel
            {
                Id = v.Id,
                NumeroVuelo = v.NumeroVuelo,
                AerolineaNombre = v.AerolineaNombre,
                OrigenIATA = v.AeropuertoOrigenIATA,
                DestinoIATA = v.AeropuertoDestinoIATA,
                HoraProgramada = esSalida ? v.FechaSalidaProgramada : v.FechaLlegadaProgramada,
                HoraEstimada = esSalida ? v.FechaSalidaProgramada : v.FechaLlegadaProgramada,
                PuertaEmbarque = Puerta(v),
                Estado = MapearEstado(v.EstadoActual, v.FechaSalidaProgramada, ahora)
            }).OrderBy(f => f.HoraProgramada).ToList();
        }

        private static EstadoFids MapearEstado(int estadoApi, DateTimeOffset fechaSalida, DateTimeOffset ahora)
        {
            return estadoApi switch
            {
                6 => EstadoFids.Cancelado,
                1 => EstadoFids.Retrasado,
                3 or 4 or 5 => EstadoFids.EnHora,
                2 => EsUltimoLlamado(fechaSalida, ahora) ? EstadoFids.UltimoLlamado : EstadoFids.Embarcando,
                _ => EstadoFids.Programado
            };
        }

        private static bool EsUltimoLlamado(DateTimeOffset fechaSalida, DateTimeOffset ahora)
        {
            var minutos = (fechaSalida - ahora).TotalMinutes;
            return minutos is >= -5 and <= 30;
        }

        private static string Puerta(VueloFidsDto vuelo)
        {
            return "A" + (vuelo.Id.GetHashCode() & 0xFFFF) % 30;
        }
    }
}
