namespace SIV.Presentation.WebUser.ViewModels.Vuelos
{
    public class AeropuertoViewModel
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoIATA { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
