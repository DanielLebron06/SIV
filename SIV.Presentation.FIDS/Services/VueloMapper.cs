using SIV.Presentation.FIDS.Services.Dtos;
using SIV.Presentation.FIDS.ViewModels;

namespace SIV.Presentation.FIDS.Services
{
    public static class VueloMapper
    {
        public static List<FilaVueloViewModel> Mapear(List<VueloFidsDto> vuelos, TipoPantallaFids tipoPantalla, string aeropuertoLocal, DateTimeOffset ahora)
        {
            var local = aeropuertoLocal.Trim().ToUpperInvariant();

            return vuelos.Select(v =>
            {
                var esSalida = EsSalida(v, tipoPantalla, local);

                return new FilaVueloViewModel
                {
                    Id = v.Id,
                    NumeroVuelo = v.NumeroVuelo,
                    AerolineaNombre = v.AerolineaNombre,
                    OrigenIATA = v.AeropuertoOrigenIATA,
                    DestinoIATA = v.AeropuertoDestinoIATA,
                    HoraProgramada = esSalida ? v.FechaSalidaProgramada : v.FechaLlegadaProgramada,
                    HoraEstimada = esSalida
                        ? v.SalidaActualizada ?? v.FechaSalidaProgramada
                        : v.LlegadaActualizada ?? v.FechaLlegadaProgramada,
                    PuertaEmbarque = esSalida
                        ? (string.IsNullOrWhiteSpace(v.PuertaEmbarque) ? "-" : v.PuertaEmbarque)
                        : "-",
                    BandaEquipaje = esSalida
                        ? "-"
                        : (string.IsNullOrWhiteSpace(v.BandaEquipaje) ? "-" : v.BandaEquipaje),
                    Terminal = esSalida
                        ? "-"
                        : (string.IsNullOrWhiteSpace(v.Terminal) ? "-" : v.Terminal),
                    EsSalida = esSalida,
                    Estado = MapearEstado(v.EstadoActual, esSalida ? v.FechaSalidaProgramada : v.FechaLlegadaProgramada, ahora)
                };
            }).OrderBy(f => f.HoraProgramada).ToList();
        }

        private static bool EsSalida(VueloFidsDto vuelo, TipoPantallaFids tipoPantalla, string aeropuertoLocal)
        {
            if (tipoPantalla == TipoPantallaFids.Salidas) return true;
            if (tipoPantalla == TipoPantallaFids.Llegadas) return false;

            if (string.Equals(vuelo.AeropuertoOrigenIATA, aeropuertoLocal, StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(vuelo.AeropuertoDestinoIATA, aeropuertoLocal, StringComparison.OrdinalIgnoreCase)) return false;

            return true;
        }

        private static EstadoFids MapearEstado(int estadoApi, DateTimeOffset horaReferencia, DateTimeOffset ahora)
        {
            return estadoApi switch
            {
                6 => EstadoFids.Cancelado,
                1 => EstadoFids.Retrasado,
                3 or 4 or 5 => EstadoFids.EnHora,
                2 => EsUltimoLlamado(horaReferencia, ahora) ? EstadoFids.UltimoLlamado : EstadoFids.Embarcando,
                _ => EstadoFids.Programado
            };
        }

        private static bool EsUltimoLlamado(DateTimeOffset horaReferencia, DateTimeOffset ahora)
        {
            var minutos = (horaReferencia - ahora).TotalMinutes;
            return minutos is >= -5 and <= 30;
        }
    }
}
