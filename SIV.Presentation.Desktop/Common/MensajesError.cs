using System;
using System.Net.Http;

namespace SIV.Presentation.Desktop.Common
{
    public static class MensajesError
    {
        public static string ObtenerMensaje(Exception ex)
        {
            if (ex is ApiException apiEx)
            {
                switch (apiEx.StatusCode)
                {
                    case 401:
                        return "Correo electrónico o contraseña incorrectos.";
                    case 403:
                        return "No tienes permisos para realizar esta acción.";
                    case 404:
                        return "El recurso solicitado no fue encontrado.";
                }

                if (!string.IsNullOrWhiteSpace(apiEx.Message))
                    return apiEx.Message;

                return "Ocurrió un error al procesar la solicitud. Intente nuevamente.";
            }

            if (ex is HttpRequestException || ex is System.Net.WebException || ex is System.Threading.Tasks.TaskCanceledException)
                return "No se pudo conectar con el servidor. Verifique su conexión.";

            return string.IsNullOrWhiteSpace(ex.Message) ? "Ocurrió un error inesperado." : ex.Message;
        }
    }
}
