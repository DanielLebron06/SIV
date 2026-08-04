using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginDTO login);

        void Logout();
    }
}
