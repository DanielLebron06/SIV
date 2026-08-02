using FluentValidation;
using SIV.Domain.Exceptions;
using SIV.Presentation.WebApi.Common;

namespace SIV.Presentation.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (DomainException ex)
            {
                await EscribirErrorAsync(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ValidationException ex)
            {
                var errores = ex.Errors.Select(e => e.ErrorMessage).ToList();
                await EscribirErrorAsync(context, StatusCodes.Status400BadRequest, "Errores de validación.", errores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado en la API.");
                await EscribirErrorAsync(context, StatusCodes.Status500InternalServerError, "Ocurrió un error interno en el servidor.");
            }
        }

        private static async Task EscribirErrorAsync(HttpContext context, int statusCode, string message, List<string>? errors = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var cuerpo = System.Text.Json.JsonSerializer.Serialize(ApiResponse.Error(message, errors));
            await context.Response.WriteAsync(cuerpo);
        }
    }
}
