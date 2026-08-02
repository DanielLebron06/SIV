using SIV.Presentation.Desktop.Services.Dtos;
using System.Threading.Tasks;

namespace SIV.Presentation.Desktop.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginDTO login);

        void Logout();
    }
}
