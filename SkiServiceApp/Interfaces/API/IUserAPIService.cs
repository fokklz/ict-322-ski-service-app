using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Interfaces.API
{
    public interface IUserAPIService : IBaseAPIService<CreateUserRequest, UpdateUserRequest, UserResponse>
    {
        Task<HTTPResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<HTTPResponse<LoginResponse>> RefreshAsync(RefreshRequest request);
        Task<HTTPResponse<UserResponse>> RevokeAsync();
        Task<HTTPResponse<UserResponse>> UnlockUser(int userId);
    }
}