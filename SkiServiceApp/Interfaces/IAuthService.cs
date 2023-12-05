using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;
using System.Windows.Input;

namespace SkiServiceApp.Interfaces
{
    public interface IAuthService
    {
        bool IsLoggedIn { get; }

        Task<(HTTPResponse<LoginResponse>, ICommand)> LoginAsync(string username, string password, bool rememberMe);
        Task<(HTTPResponse<LoginResponse>, ICommand)> LoginAsyncWithToken(string token, string refreshToken);
        Task LogoutAsync();
    }
}