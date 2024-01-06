using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;
using System.Windows.Input;

namespace SkiServiceApp.Interfaces
{
    public interface IAuthService
    { 
        Task<HTTPResponse<LoginResponse>> LoginAsync(string username, string password, bool rememberMe);
        Task<HTTPResponse<LoginResponse>> LoginAsyncWithToken(string token, string refreshToken);
        Task LogoutAsync(bool force = false);
    }
}