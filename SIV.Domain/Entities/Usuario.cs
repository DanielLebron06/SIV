using System;
using System.Collections.Generic;
using SIV.Domain.Emuns;

namespace SIV.Domain.Entities
{
    public class Usuario
    {
        private const int MaximoIntentosFallidos = 5;
        private static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(15);

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Rol Rol { get; set; }
        public bool Activo { get; set; } = true;
        public int IntentosFallidos { get; set; }
        public DateTimeOffset? BloqueadoHasta { get; set; }

        public List<SeguimientoVuelo> Seguimientos { get; set; } = new List<SeguimientoVuelo>();

        public void RegistrarIntentoExitoso()
        {
            IntentosFallidos = 0;
            BloqueadoHasta = null;
        }

        public bool RegistrarIntentoFallido(DateTimeOffset momentoActual)
        {
            if (momentoActual >= BloqueadoHasta)
            {
                BloqueadoHasta = null;
                IntentosFallidos = 0;
            }

            IntentosFallidos++;

            if (IntentosFallidos >= MaximoIntentosFallidos)
            {
                BloqueadoHasta = momentoActual.Add(DuracionBloqueo);
                IntentosFallidos = 0;
                return true;
            }

            return false;
        }

        public bool EstaBloqueado(DateTimeOffset momentoActual)
        {
            return BloqueadoHasta.HasValue && BloqueadoHasta.Value > momentoActual;
        }

        public int IntentosRestantes(DateTimeOffset momentoActual)
        {
            if (EstaBloqueado(momentoActual))
            {
                return 0;
            }
            return Math.Max(0, MaximoIntentosFallidos - IntentosFallidos);
        }
    }
}
