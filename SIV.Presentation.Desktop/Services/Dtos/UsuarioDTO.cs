using System;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public Rol Rol { get; set; }
    }

    public class UsuarioInternoDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public Rol Rol { get; set; }
        public bool Activo { get; set; }
    }
}
