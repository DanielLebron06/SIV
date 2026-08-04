using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Vuelos
{
    public interface IVueloService
    {
        Task<List<VueloDTO>> ObtenerVuelosAsync(FiltrosVuelos filtros);
        Task<VueloDTO> ObtenerVueloAsync(Guid id);
        Task CrearVueloAsync(DatosVueloDTO vuelo);
        Task ActualizarVueloAsync(Guid id, DatosVueloDTO vuelo);
        Task ActualizarEstadoAsync(Guid id, EstadoVuelo nuevoEstado);
        Task<List<EstadoVueloDTO>> ObtenerHistorialEstadosAsync(Guid id);
        Task<List<CambioOperativoDTO>> ObtenerHistorialCambiosAsync(Guid id);
        Task CancelarVueloAsync(Guid id, string motivo);
        Task RegistrarRetrasoAsync(Guid id, DateTimeOffset nuevaHoraEstimada, string motivo);
        Task RegistrarAdelantoAsync(Guid id, DateTimeOffset nuevaHoraEstimada, string motivo);
        Task RegistrarCambioPuertaAsync(Guid id, string nuevaPuerta, string motivo);
        Task AsignarPuertaInicialAsync(Guid id, string puerta);
    }
}
