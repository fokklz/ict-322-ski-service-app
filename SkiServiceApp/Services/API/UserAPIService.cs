using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Services.API
{
    public class UserAPIService : BaseAPIService<CreateUserRequest, UpdateUserRequest, UserResponse>, IUserAPIService
    {
        public UserAPIService(IConfiguration configuration) : base(configuration, "users")
        {
        }

        /// <summary>
        /// Login a user to the application 
        /// - The reveived token is stored in the storage service
        /// </summary>
        /// <param name="request">The login information to use</param>
        /// <returns>The login response</returns>
        public async Task<HTTPResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var res = await _sendRequest(HttpMethod.Post, _url("login"), request);
            return new HTTPResponse<LoginResponse>(res);
        }

        /// <summary>
        /// Refresh a user's token
        /// </summary>
        /// <param name="request">The refresh-login information to use</param>
        /// <returns>The login response</returns>
        public async Task<HTTPResponse<LoginResponse>> RefreshAsync(RefreshRequest request)
        {
            var res = await _sendRequest(HttpMethod.Post, _url("refresh"), request);
            return new HTTPResponse<LoginResponse>(res);
        }

        /// <summary>
        /// Unlock a user
        /// </summary>
        /// <param name="userId">The user to unlock</param>
        /// <returns>The Information of the unlocked user</returns>
        public async Task<HTTPResponse<UserResponse>> UnlockUser(int userId)
        {
            var res = await _sendRequest(HttpMethod.Post, _url(userId.ToString(), "unlock"));
            return new HTTPResponse<UserResponse>(res);
        }
    }
}
