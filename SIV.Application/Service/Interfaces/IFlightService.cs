using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Application.DTOs;

namespace SIV.Application.Service.Interfaces
{
    public interface IFlightService
    {
        Task RegistrarVuelo(DatosVuelo datos, Usuario usuario);
        Task RegistrarAerolinea(AerolineaDTO datos, Usuario usuario);
        Task RegistrarAeropuerto(AeropuertoDTO datos, Usuario usuario);
        Task ActualizarVuelo(Guid vueloId, DatosVuelo datos, Usuario usuario);
        Task<List<Aerolinea>> ObtenerAerolineas(Usuario usuario);
        Task<List<Aeropuerto>> ObtenerAeropuerto(Usuario usuario);
        Task DesactivarAeropuerto(Guid AeropuertoId, Usuario usuario);
        Task DesactivarAerolinea(Guid AerolineaId, Usuario usuario);
        Task<List<Vuelo>> ConsultarVuelos(FiltrosVuelos filtros, Usuario usuario);
        Task CambiarEstadoVuelo(Guid vueloId, EstadoVuelo nuevoEstado, Usuario usuari);
        Task<List<HistorialEstado>> ObtenerEstadosVuelo(Guid VueloId, Usuario usuario);

    }
}
