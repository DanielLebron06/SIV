using SIV.Application.DTOs.Aerolinea;
using SIV.Application.DTOs.Aeropuerto;
using SIV.Application.DTOs.Vuelo;
using SIV.Domain.Common;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;


namespace SIV.Application.Service.Interfaces
{
    public interface IFlightService
    {

        Task RegistrarVuelo(DatosVueloDTO datos, Usuario usuario);

        Task RegistrarAerolinea(RegistroAerolineaDTO datos, Usuario usuario);

        Task RegistrarAeropuerto(RegistroAeropuertoDTO datos, Usuario usuario);

        Task ActualizarVuelo(Guid vueloId, DatosVueloDTO datos, Usuario usuario);

        Task<List<AerolineaDTO>> ObtenerAerolineas(Usuario usuario);

        Task<List<AeropuertoDTO>> ObtenerAeropuertos(Usuario usuario);

        Task DesactivarAeropuerto(Guid aeropuertoId, Usuario usuario);

        Task DesactivarAerolinea(Guid aerolineaId, Usuario usuario);

        Task<List<DatosVueloDTO>> ConsultarVuelos(
            FiltrosVuelos filtros,
            Usuario usuario);

        Task CambiarEstadoVuelo(
            Guid vueloId,
            EstadoVuelo nuevoEstado,
            Usuario usuario);

        Task<List<HistorialEstadoDTO>> ObtenerEstadosVuelo(
            Guid vueloId,
            Usuario usuario);

        Task<Vuelo> ObtenerVuelo(Guid id);


    }
}
