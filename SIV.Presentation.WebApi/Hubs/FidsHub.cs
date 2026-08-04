using Microsoft.AspNetCore.SignalR;
using SIV.Application.DTOs.Fids;

namespace SIV.Presentation.WebApi.Hubs
{
    /// <summary>
    /// Interfaz del cliente para recibir actualizaciones de vuelos del tablero FIDS.
    /// </summary>
    public interface IFidsClient
    {
        Task RecibirActualizacionVuelo(DtoFidsVuelo vuelo);
    }

    /// <summary>
    /// Hub de SignalR para transmitir actualizaciones de vuelos a las pantallas FIDS.
    /// </summary>
    public class FidsHub : Hub<IFidsClient>
    {
        private const string GrupoSalidas = "salidas";
        private const string GrupoLlegadas = "llegadas";
        private const string GrupoGeneral = "general";

        /// <summary>
        /// Inscribe la conexión en el grupo de salidas.
        /// </summary>
        public async Task UnirseASalidas()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GrupoSalidas);
        }

        /// <summary>
        /// Inscribe la conexión en el grupo de llegadas.
        /// </summary>
        public async Task UnirseALlegadas()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GrupoLlegadas);
        }

        /// <summary>
        /// Inscribe la conexión en el grupo general.
        /// </summary>
        public async Task UnirseAGeneral()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GrupoGeneral);
        }

        /// <summary>
        /// Retira la conexión del grupo de salidas.
        /// </summary>
        public async Task SalirDeSalidas()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoSalidas);
        }

        /// <summary>
        /// Retira la conexión del grupo de llegadas.
        /// </summary>
        public async Task SalirDeLlegadas()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoLlegadas);
        }

        /// <summary>
        /// Retira la conexión del grupo general.
        /// </summary>
        public async Task SalirDeGeneral()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoGeneral);
        }
    }
}
