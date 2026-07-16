using SIV.Domain.Emuns;
using SIV.Domain.Entities;

namespace SIV.Application.Common.Extensions
{
    /// <summary>
    /// Métodos de extensión para la entidad Usuario que simplifican las validaciones de acceso y roles.
    /// </summary>
    public static class UsuarioExtensions
    {
        /// <summary>
        /// Verifica si el usuario tiene rol de Administrador.
        /// </summary>
        public static bool EsAdministrador(this Usuario usuario)
        {
            return usuario.Rol == Rol.Administrador;
        }

        /// <summary>
        /// Verifica si el usuario tiene rol de Operador.
        /// </summary>
        public static bool EsOperador(this Usuario usuario)
        {
            return usuario.Rol == Rol.Operador;
        }

        /// <summary>
        /// Verifica si el usuario tiene rol de Operador o Administrador.
        /// </summary>
        public static bool EsOperadorOAdministrador(this Usuario usuario)
        {
            return usuario.Rol == Rol.Operador || usuario.Rol == Rol.Administrador;
        }

        /// <summary>
        /// Verifica si el usuario tiene rol de Auditor o Administrador.
        /// </summary>
        public static bool EsAuditorOAdministrador(this Usuario usuario)
        {
            return usuario.Rol == Rol.Auditor || usuario.Rol == Rol.Administrador;
        }
        
        /// <summary>
        /// Verifica si el usuario es un UsuarioRegistrado.
        /// </summary>
        public static bool EsUsuarioRegistrado(this Usuario usuario)
        {
            return usuario.Rol == Rol.UsuarioRegistrado;
        }
    }
}
