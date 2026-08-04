using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Common
{
    public class SignalRClient
    {
        private HubConnection _connection;
        public event Action<string> OnConnectionStatusChanged;
        public event Action OnVuelosUpdated;
        public event Action<string, string> OnNotificationReceived;

        public async Task ConnectAsync(string token)
        {
            if (_connection != null && _connection.State != HubConnectionState.Disconnected)
                return;

            var builder = new HubConnectionBuilder()
                .WithUrl("https://localhost:7001/hubs/vuelos", options =>
                {
                    if (!string.IsNullOrEmpty(token))
                        options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect();

            _connection = builder.Build();

            _connection.Closed += async error =>
            {
                OnConnectionStatusChanged?.Invoke("Desconectado");
                await Task.CompletedTask;
            };

            _connection.Reconnecting += async error =>
            {
                OnConnectionStatusChanged?.Invoke("Reconectando...");
                await Task.CompletedTask;
            };

            _connection.Reconnected += async connectionId =>
            {
                OnConnectionStatusChanged?.Invoke("Conectado");
                await Task.CompletedTask;
            };

            _connection.On("VuelosActualizados", () => OnVuelosUpdated?.Invoke());
            _connection.On<Guid>("VueloCancelado", id => OnVuelosUpdated?.Invoke());
            _connection.On<Guid>("VueloRetrasado", id => OnVuelosUpdated?.Invoke());
            _connection.On<Guid>("VueloAdelantado", id => OnVuelosUpdated?.Invoke());
            _connection.On<Guid>("VueloCambioPuerta", id => OnVuelosUpdated?.Invoke());
            _connection.On<Guid, string>("VueloEstadoCambiado", (id, estado) => OnVuelosUpdated?.Invoke());
            _connection.On<string, string>("RecibirNotificacion", (titulo, mensaje) => OnNotificationReceived?.Invoke(titulo, mensaje));

            try
            {
                await _connection.StartAsync();
                OnConnectionStatusChanged?.Invoke("Conectado");
            }
            catch
            {
                OnConnectionStatusChanged?.Invoke("Error de Conexion");
            }
        }

        public async Task DisconnectAsync()
        {
            if (_connection != null && _connection.State != HubConnectionState.Disconnected)
            {
                await _connection.StopAsync();
                OnConnectionStatusChanged?.Invoke("Desconectado");
            }
        }
    }
}
