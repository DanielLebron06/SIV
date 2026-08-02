using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Services.Implementations
{
    public class VueloService : IVueloService
    {
        private readonly ApiClient _apiClient;

        public VueloService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<VueloDTO>> ObtenerVuelosAsync(FiltrosVuelos filtros)
        {
            var query = QueryBuilder.Build(new Dictionary<string, object>
            {
                { "AerolineaId", filtros?.AerolineaId },
                { "AeropuertoOrigenId", filtros?.AeropuertoOrigenId },
                { "AeropuertoDestinoId", filtros?.AeropuertoDestinoId },
                { "Fecha", filtros?.Fecha },
                { "Estado", filtros?.Estado }
            });

            return await _apiClient.GetAsync<List<VueloDTO>>(
                "Vuelos" + (string.IsNullOrEmpty(query) ? string.Empty : "?" + query));
        }

        public async Task<VueloDTO> ObtenerVueloAsync(Guid id)
        {
            return await _apiClient.GetAsync<VueloDTO>($"Vuelos/{id}");
        }

        public async Task CrearVueloAsync(DatosVueloDTO vuelo)
        {
            await _apiClient.PostAsync("Vuelos", vuelo);
        }

        public async Task ActualizarVueloAsync(Guid id, DatosVueloDTO vuelo)
        {
            await _apiClient.PutAsync($"Vuelos/{id}", vuelo);
        }

        public async Task ActualizarEstadoAsync(Guid id, EstadoVuelo nuevoEstado)
        {
            await _apiClient.PutAsync($"Vuelos/{id}/estado", new ActualizarEstadoDTO { NuevoEstado = nuevoEstado });
        }

        public async Task<List<EstadoVueloDTO>> ObtenerHistorialEstadosAsync(Guid id)
        {
            return await _apiClient.GetAsync<List<EstadoVueloDTO>>($"Vuelos/{id}/historial");
        }

        public async Task<List<CambioOperativoDTO>> ObtenerHistorialCambiosAsync(Guid id)
        {
            return await _apiClient.GetAsync<List<CambioOperativoDTO>>($"Vuelos/{id}/cambios");
        }

        public async Task CancelarVueloAsync(Guid id, string motivo)
        {
            await _apiClient.PutAsync($"Vuelos/{id}/cancelar", new CancelarVueloDTO { Motivo = motivo });
        }

        public async Task RegistrarRetrasoAsync(Guid id, DateTimeOffset nuevaHoraEstimada, string motivo)
        {
            await _apiClient.PutAsync($"Vuelos/{id}/retraso", new CambioOperativoTiempoDTO
            {
                NuevaHoraEstimada = nuevaHoraEstimada,
                Motivo = motivo
            });
        }

        public async Task RegistrarAdelantoAsync(Guid id, DateTimeOffset nuevaHoraEstimada, string motivo)
        {
            await _apiClient.PutAsync($"Vuelos/{id}/adelanto", new CambioOperativoTiempoDTO
            {
                NuevaHoraEstimada = nuevaHoraEstimada,
                Motivo = motivo
            });
        }

        public async Task RegistrarCambioPuertaAsync(Guid id, string nuevaPuerta, string motivo)
        {
            await _apiClient.PutAsync($"Vuelos/{id}/puerta", new CambioPuertaDTO
            {
                NuevaPuerta = nuevaPuerta,
                Motivo = motivo
            });
        }
    }
}
