namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class RegistroUsuarioDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegistroUsuarioInternoDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Rol Rol { get; set; }
    }
}
