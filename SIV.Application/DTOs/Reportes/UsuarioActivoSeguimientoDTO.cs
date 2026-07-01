namespace SIV.Application.DTOs.Reportes
{
    public class UsuarioActivoSeguimientoDTO
    {
        public Guid UsuarioId { get; set; }

        public string Email { get; set; } = string.Empty;

        public int TotalSeguimientos { get; set; }
    }
}