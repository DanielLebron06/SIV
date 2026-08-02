using System.Collections.Generic;

namespace SIV.Presentation.WebApi.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse Error(string? message, List<string>? errors = null)
            => new() { Success = false, Message = string.IsNullOrWhiteSpace(message) ? "Ocurrió un error." : message, Errors = errors };

        public static ApiResponse Ok(string message)
            => new() { Success = true, Message = message };
    }
}
