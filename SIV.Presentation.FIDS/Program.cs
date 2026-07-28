using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SIV.Presentation.FIDS;

public class Program
{
    private static List<VueloDTO> _vuelos = new();
    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:7001/") // Cambia por el puerto real de tu WebAPI
    };

    static async Task Main(string[] args)
    {
        Console.Title = "FIDS - Pantalla de Vuelos Aeropuerto";

        // 1. Cargar datos iniciales desde el endpoint GET api/v1/vuelos
        await CargarVuelosInicialesAsync();

        // 2. Configurar la conexión con el Hub de SignalR
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7001/hubs/vuelos") // La misma ruta configurada en Program.cs de la API
            .WithAutomaticReconnect()
            .Build();

        // 3. Escuchar el evento de cambio de estado
        connection.On<Guid, int>("VueloEstadoCambiado", (vueloId, nuevoEstado) =>
        {
            var vuelo = _vuelos.FirstOrDefault(v => v.Id == vueloId);
            if (vuelo != null)
            {
                vuelo.EstadoActual = nuevoEstado;
                RenderizarTablero(); // Redibujamos la consola con el nuevo estado y color
            }
        });

        try
        {
            await connection.StartAsync();
            RenderizarTablero();

            Console.WriteLine("\n[SignalR Conectado] Presiona ENTER para cerrar el FIDS...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar a SignalR: {ex.Message}");
        }
    }

    private static async Task CargarVuelosInicialesAsync()
    {
        try
        {
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Permite mapear id con Id, destino con Destino, etc.
            };

            var response = await _httpClient.GetFromJsonAsync<List<VueloDTO>>("api/v1/vuelos", opciones);

            if (response != null)
            {
                _vuelos = response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error al cargar vuelos iniciales]: {ex.Message}");
        }
    }

    private static void RenderizarTablero()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=========================================================================================");
        Console.WriteLine("                        AEROPUERTO INTERNACIONAL - PANTALLA FIDS                        ");
        Console.WriteLine($"                        Última actualización: {DateTime.Now:HH:mm:ss}                             ");
        Console.WriteLine("=========================================================================================");
        Console.ResetColor();

        Console.WriteLine($"{"VUELO",-10} | {"HORA SALIDA",-12} | {"ESTADO",-15}");
        Console.WriteLine(new string('-', 45));

        foreach (var vuelo in _vuelos)
        {
            // Asignar color según el número de estado
            Console.ForegroundColor = vuelo.EstadoActual switch
            {
                0 => ConsoleColor.Gray,       // Programado
                1 => ConsoleColor.Green,      // A tiempo
                2 => ConsoleColor.Yellow,     // Embarcando
                3 => ConsoleColor.Cyan,       // En Vuelo
                4 => ConsoleColor.Red,        // Retrasado
                5 => ConsoleColor.DarkRed,    // Cancelado
                _ => ConsoleColor.White
            };

            // Imprimimos la hora formateada HH:mm y el texto del estado
            Console.WriteLine($"{vuelo.NumeroVuelo,-10} | {vuelo.FechaSalidaProgramada,-12:HH:mm} | {vuelo.EstadoTexto,-15}");
            Console.ResetColor();
        }

        Console.WriteLine("=========================================================================================");
    }
}

// DTO local sencillo para mapear lo que devuelve la API
public class VueloDTO
{
    [JsonPropertyName("id")] // O "vueloId" según cómo lo envíe tu API
    public Guid Id { get; set; }

    [JsonPropertyName("numeroVuelo")]
    public string NumeroVuelo { get; set; } = string.Empty;

    [JsonPropertyName("aeropuertoOrigenId")]
    public Guid AeropuertoOrigenId { get; set; }

    [JsonPropertyName("aeropuertoDestinoId")]
    public Guid AeropuertoDestinoId { get; set; }

    [JsonPropertyName("estadoActual")]
    public int EstadoActual { get; set; }

    [JsonPropertyName("fechaSalidaProgramada")]
    public DateTime FechaSalidaProgramada { get; set; }

    [JsonPropertyName("fechaLlegadaProgramada")]
    public DateTime FechaLlegadaProgramada { get; set; }

    // Convierte el entero del enum (0, 1, 2...) a un texto amigable
    public string EstadoTexto => EstadoActual switch
    {
        0 => "Programado",
        1 => "A Tiempo",
        2 => "Embarcando",
        3 => "En Vuelo",
        4 => "Retrasado",
        5 => "Cancelado",
        _ => "Desconocido"
    };
}