using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Hub de SignalR para transmitir actualizaciones de vuelos a clientes conectados
/// </summary>
namespace SIV.Presentation.WebApi.Hubs
{
    public class VuelosHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
