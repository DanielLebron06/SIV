using System;
using System.Collections.Generic;

namespace SIV.Presentation.Desktop.Services.Dtos
{
    public class VueloCambiosDTO
    {
        public Guid VueloId { get; set; }
        public List<CambioOperativoDTO> Cambios { get; set; } = new List<CambioOperativoDTO>();
    }
}
