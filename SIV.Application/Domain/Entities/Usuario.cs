using System;
using System.Collections.Generic;
using SIV.Application.Common;

namespace SIV.Application.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty; // Único en el sistema (RN-USU-01)
        public string PasswordHash { get; set; } = string.Empty; // Hash BCrypt requerido en SAD
        public Rol Rol { get; set; }
        public bool Activo { get; set; } = true;

        public List<SeguimientoVuelo> Seguimientos { get; set; } = new List<SeguimientoVuelo>();
    }
}