using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7001/api/v1/";
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private async Task HandleErrorResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new ApiException((int)response.StatusCode, ExtraerMensajeServidor(content));
            }
        }

        private static string ExtraerMensajeServidor(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            try
            {
                using (var doc = JsonDocument.Parse(content))
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.String)
                        return doc.RootElement.GetString();

                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty("mensaje", out var mensaje) &&
                        mensaje.ValueKind == JsonValueKind.String)
                        return mensaje.GetString();
                }
            }
            catch (JsonException)
            {
            }

            return string.Empty;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            await HandleErrorResponse(response);
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
                return default;
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        public async Task<byte[]> GetBytesAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            await HandleErrorResponse(response);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            await HandleErrorResponse(response);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent))
                return default;
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
        }

        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            await HandleErrorResponse(response);
        }

        public async Task PutAsync(string endpoint)
        {
            var response = await _httpClient.PutAsync(endpoint, null);
            await HandleErrorResponse(response);
        }

        public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            await HandleErrorResponse(response);
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            await HandleErrorResponse(response);
        }
    }
}
