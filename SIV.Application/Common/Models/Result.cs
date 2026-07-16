using System;

namespace SIV.Application.Common.Models
{
    /// <summary>
    /// Representa el resultado de una operación.
    /// Contiene un estado de éxito o fracaso, y opcionalmente un mensaje de error.
    /// </summary>
    /// <summary>
    /// Representa el resultado de una operación que devuelve datos de tipo T.
    /// </summary>
    /// <typeparam name="T">El tipo de dato que devuelve la operación en caso de éxito.</typeparam>
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
