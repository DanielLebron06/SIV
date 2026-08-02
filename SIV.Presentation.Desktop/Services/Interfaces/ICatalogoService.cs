using SIV.Presentation.Desktop.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Services.Interfaces
{
    public interface ICatalogoService
    {
        Task<List<AerolineaDTO>> ObtenerAerolineasAsync();
        Task RegistrarAerolineaAsync(RegistroAerolineaDTO aerolinea);
        Task DesactivarAerolineaAsync(Guid id);
        Task<List<AeropuertoDTO>> ObtenerAeropuertosAsync();
        Task RegistrarAeropuertoAsync(RegistroAeropuertoDTO aeropuerto);
        Task DesactivarAeropuertoAsync(Guid id);
    }
}
