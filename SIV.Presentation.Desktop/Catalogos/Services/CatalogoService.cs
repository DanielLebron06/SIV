using SIV.Presentation.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Catalogos
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ApiClient _apiClient;

        public CatalogoService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<AerolineaDTO>> ObtenerAerolineasAsync()
        {
            return await _apiClient.GetAsync<List<AerolineaDTO>>("Catalogos/aerolineas");
        }

        public async Task RegistrarAerolineaAsync(RegistroAerolineaDTO aerolinea)
        {
            await _apiClient.PostAsync("Catalogos/aerolineas", aerolinea);
        }

        public async Task DesactivarAerolineaAsync(Guid id)
        {
            await _apiClient.PutAsync($"Catalogos/aerolineas/{id}/desactivar");
        }

        public async Task<List<AeropuertoDTO>> ObtenerAeropuertosAsync()
        {
            return await _apiClient.GetAsync<List<AeropuertoDTO>>("Catalogos/aeropuertos");
        }

        public async Task RegistrarAeropuertoAsync(RegistroAeropuertoDTO aeropuerto)
        {
            await _apiClient.PostAsync("Catalogos/aeropuertos", aeropuerto);
        }

        public async Task DesactivarAeropuertoAsync(Guid id)
        {
            await _apiClient.PutAsync($"Catalogos/aeropuertos/{id}/desactivar");
        }
    }
}
