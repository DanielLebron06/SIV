using System.Net;

namespace SIV.Presentation.WebUser.Services.Common
{
    public static class MensajesError
    {
        public static string ObtenerMensaje(Exception ex)
        {
            return ex switch
            {
                ApiException api when api.StatusCode == (int)HttpStatusCode.Unauthorized =>
                    "Debes iniciar sesión para realizar esta acción.",
                ApiException api when api.StatusCode == (int)HttpStatusCode.Forbidden =>
                    "No tienes permisos para realizar esta acción.",
                ApiException api when api.StatusCode == (int)HttpStatusCode.NotFound =>
                    "El recurso solicitado no fue encontrado.",
                ApiException api when api.StatusCode == (int)HttpStatusCode.BadRequest =>
                    api.Message,
                ApiException api =>
                    api.Message,
                HttpRequestException =>
                    "No se pudo conectar con el servidor. Inténtalo nuevamente.",
                TaskCanceledException =>
                    "El servidor tardó demasiado en responder. Inténtalo nuevamente.",
                _ =>
                    "Ocurrió un error inesperado. Inténtalo nuevamente."
            };
        }
    }
}
