namespace SIV.Application.Common.Security
{
    /// <summary>
    /// Clase de utilidades para el manejo seguro de contraseñas usando BCrypt.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Genera un hash seguro para la contraseña proporcionada.
        /// </summary>
        /// <param name="password">La contraseña en texto plano.</param>
        /// <returns>El hash de la contraseña.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con un hash existente.
        /// </summary>
        /// <param name="password">La contraseña en texto plano introducida por el usuario.</param>
        /// <param name="hash">El hash almacenado en la base de datos.</param>
        /// <returns>True si las contraseñas coinciden; False en caso contrario.</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
