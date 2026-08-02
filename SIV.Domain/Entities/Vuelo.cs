using SIV.Domain.Emuns;
using SIV.Domain.Exceptions;

namespace SIV.Domain.Entities
{
    public class Vuelo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AerolineaId { get; set; }
        public Guid AeropuertoOrigenId { get; set; }
        public Guid AeropuertoDestinoId { get; set; }
        public Aerolinea? Aerolinea { get; set; }
        public Aeropuerto? AeropuertoOrigen { get; set; }
        public Aeropuerto? AeropuertoDestino { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public EstadoVuelo EstadoActual { get; set; } = EstadoVuelo.Programado;

        public DateTimeOffset SalidaPlanificada { get; set; }
        public DateTimeOffset LlegadaPlanificada { get; set; }

        public DateTimeOffset? SalidaActualizada { get; set; }
        public DateTimeOffset? LlegadaActualizada { get; set; }
        public string? PuertaEmbarque { get; set; }

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public Guid CreadoPorId { get; set; }

        public List<CambioOperativo> CambiosOperativos { get; set; } = new List<CambioOperativo>();
        public List<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();
        public List<SeguimientoVuelo> Seguidores { get; set; } = new List<SeguimientoVuelo>();
        public List<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

        public static Vuelo Crear(
            string numeroVuelo,
            Guid aerolineaId,
            Guid aeropuertoOrigenId,
            Guid aeropuertoDestinoId,
            DateTimeOffset salidaPlanificada,
            DateTimeOffset llegadaPlanificada,
            Guid creadoPorId)
        {
            if (aeropuertoOrigenId == aeropuertoDestinoId)
            {
                throw new DomainException("El aeropuerto de origen no puede ser el mismo que el de destino.");
            }

            if (llegadaPlanificada <= salidaPlanificada)
            {
                throw new DomainException("La llegada planificada debe ser posterior a la salida planificada.");
            }

            return new Vuelo
            {
                Id = Guid.NewGuid(),
                NumeroVuelo = numeroVuelo,
                AerolineaId = aerolineaId,
                AeropuertoOrigenId = aeropuertoOrigenId,
                AeropuertoDestinoId = aeropuertoDestinoId,
                EstadoActual = EstadoVuelo.Programado,
                SalidaPlanificada = salidaPlanificada,
                LlegadaPlanificada = llegadaPlanificada,
                CreadoPorId = creadoPorId
            };
        }

        public void CambiarEstado(EstadoVuelo nuevoEstado)
        {
            if (EsTerminal)
            {
                throw new DomainException("Un vuelo Cancelado o Completado no puede cambiar de estado.");
            }

            if (!TransicionPermitida(EstadoActual, nuevoEstado))
            {
                throw new DomainException($"No se permite la transición de {EstadoActual} a {nuevoEstado}.");
            }

            EstadoActual = nuevoEstado;
        }

        public void RegistrarRetraso(DateTimeOffset nuevaSalidaEstimada)
        {
            if (EsTerminal)
            {
                throw new DomainException("Un vuelo Cancelado o Completado no admite retrasos.");
            }

            if (EstadoActual != EstadoVuelo.Programado && EstadoActual != EstadoVuelo.Retrasado)
            {
                throw new DomainException($"No se puede registrar un retraso en un vuelo en estado {EstadoActual}.");
            }

            var nuevaLlegadaEstimada = LlegadaPlanificada + (nuevaSalidaEstimada - SalidaPlanificada);
            if (nuevaLlegadaEstimada <= nuevaSalidaEstimada)
            {
                throw new DomainException("La llegada estimada debe ser posterior a la salida estimada.");
            }

            EstadoActual = EstadoVuelo.Retrasado;
            SalidaActualizada = nuevaSalidaEstimada;
            LlegadaActualizada = nuevaLlegadaEstimada;
        }

        public void RegistrarAdelanto(DateTimeOffset nuevaSalidaEstimada)
        {
            if (EsTerminal)
            {
                throw new DomainException("Un vuelo Cancelado o Completado no admite adelantos.");
            }

            var nuevaLlegadaEstimada = LlegadaPlanificada + (nuevaSalidaEstimada - SalidaPlanificada);
            if (nuevaLlegadaEstimada <= nuevaSalidaEstimada)
            {
                throw new DomainException("La llegada estimada debe ser posterior a la salida estimada.");
            }

            SalidaActualizada = nuevaSalidaEstimada;
            LlegadaActualizada = nuevaLlegadaEstimada;
        }

        public void CambiarPuerta(string nuevaPuerta)
        {
            if (EsTerminal)
            {
                throw new DomainException("Un vuelo Cancelado o Completado no admite cambio de puerta.");
            }

            if (string.IsNullOrWhiteSpace(nuevaPuerta))
            {
                throw new DomainException("La nueva puerta de embarque es obligatoria.");
            }

            PuertaEmbarque = nuevaPuerta;
        }

        public void Cancelar()
        {
            if (EsTerminal)
            {
                throw new DomainException("Un vuelo Cancelado o Completado no puede cancelarse nuevamente.");
            }

            if (EstadoActual != EstadoVuelo.Programado &&
                EstadoActual != EstadoVuelo.Retrasado &&
                EstadoActual != EstadoVuelo.Embarcando)
            {
                throw new DomainException($"No se puede cancelar un vuelo en estado {EstadoActual}.");
            }

            EstadoActual = EstadoVuelo.Cancelado;
        }

        private bool EsTerminal => EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado;

        private static bool TransicionPermitida(EstadoVuelo origen, EstadoVuelo destino)
        {
            return (origen, destino) switch
            {
                (EstadoVuelo.Programado, EstadoVuelo.Embarcando) => true,
                (EstadoVuelo.Programado, EstadoVuelo.Retrasado) => true,
                (EstadoVuelo.Programado, EstadoVuelo.Cancelado) => true,
                (EstadoVuelo.Retrasado, EstadoVuelo.Embarcando) => true,
                (EstadoVuelo.Retrasado, EstadoVuelo.Cancelado) => true,
                (EstadoVuelo.Embarcando, EstadoVuelo.EnVuelo) => true,
                (EstadoVuelo.Embarcando, EstadoVuelo.Cancelado) => true,
                (EstadoVuelo.EnVuelo, EstadoVuelo.Aterrizado) => true,
                (EstadoVuelo.Aterrizado, EstadoVuelo.Completado) => true,
                _ => false
            };
        }
    }
}