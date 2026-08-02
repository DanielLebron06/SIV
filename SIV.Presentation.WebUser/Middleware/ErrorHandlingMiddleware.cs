using SIV.Presentation.WebUser.Exceptions;
using SIV.Presentation.WebUser.Services;

namespace SIV.Presentation.WebUser.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning(ex, "Error de la API (código {StatusCode}).", ex.StatusCode);
                await RedirigirAErrorAsync(context, ex.StatusCode, MensajesError.ObtenerMensaje(ex));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión con el servidor de la API.");
                await RedirigirAErrorAsync(context, StatusCodes.Status502BadGateway, MensajesError.ObtenerMensaje(ex));
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "La solicitud a la API superó el tiempo de espera.");
                await RedirigirAErrorAsync(context, StatusCodes.Status504GatewayTimeout, MensajesError.ObtenerMensaje(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado.");
                await RedirigirAErrorAsync(context, StatusCodes.Status500InternalServerError, MensajesError.ObtenerMensaje(ex));
            }
        }

        private static async Task RedirigirAErrorAsync(HttpContext context, int statusCode, string mensaje)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.Redirect($"/Home/Error?statusCode={statusCode}&mensaje={Uri.EscapeDataString(mensaje)}");
            await Task.CompletedTask;
        }
    }
}
