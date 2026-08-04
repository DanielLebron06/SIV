using SIV.Presentation.Desktop.Common;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApiClient _apiClient;

        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO login)
        {
            var response = await _apiClient.PostAsync<LoginDTO, LoginResponse>("Auth/login", login);
            if (response != null && !string.IsNullOrEmpty(response.Token))
                _apiClient.SetToken(response.Token);
            return response;
        }

        public void Logout()
        {
            _apiClient.ClearToken();
        }
    }
}
