using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Services.API
{
    public class UserAPIService : BaseAPIService<CreateUserRequest, UpdateUserRequest, UserResponse>, IUserAPIService
    {
        private readonly IStorageService _storageService;

        public UserAPIService(IConfiguration configuration, IStorageService storageService) : base(configuration, "users")
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Login a user to the application 
        /// - The reveived token is stored in the storage service
        /// </summary>
        /// <param name="request">The login information to use</param>
        /// <returns>The login response of the API (handled - more to be used for messaging and such)</returns>
        public async Task<HTTPResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var res = await _sendRequest(HttpMethod.Post, _url("login"), request);
            var loginResponse = new HTTPResponse<LoginResponse>(res);
            if (loginResponse.IsSuccess)
            {
                // we directly process the token here, since we need to store it in the storage service
                var parsed = await loginResponse.ParseSuccess();
                if (parsed != null && parsed.Auth != null && parsed.Auth.Token != null)
                {
                    if (parsed.Auth.RefreshToken != null)
                    {
                        _storageService.StoreUser(request.Username, parsed.Auth.Token, parsed.Auth.RefreshToken);
                    }

                    _storageService.SetToken(parsed.Auth.Token);
                    await _storageService.SaveChangesAsync();
                    return loginResponse;
                }

                // TODO: this should be handled better
                throw new Exception("Failed to parse token from login response");
            }
            return loginResponse;
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
