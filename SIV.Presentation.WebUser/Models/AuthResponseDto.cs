namespace SIV.Presentation.WebUser.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
