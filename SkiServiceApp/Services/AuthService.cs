using PropertyChanged;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;
using System.Windows.Input;

namespace SkiServiceApp.Services
{
    [AddINotifyPropertyChangedInterface]
    public class AuthService : IAuthService
    {
        private readonly IUserAPIService _userAPIService;
        private readonly IStorageService _storageService;

        public bool IsLoggedIn { get; private set; }

        public AuthService(IUserAPIService userAPIService, IStorageService storageService)
        {
            _storageService = storageService;
            _userAPIService = userAPIService;
        }

        /// <summary>
        /// Internal method to handle the login response
        /// Since both Login and Refresh use the same logic, we can use this method for both
        /// </summary>
        /// <param name="res">The response obtained by Login or Refresh</param>
        /// <param name="oldRefreshToken">used to remove this refresh token if the response turns out to be falsy</param>
        /// <returns>The Response, The command to switch to main app</returns>
        private async Task<(HTTPResponse<LoginResponse>, ICommand)> _handleLoginResponse(HTTPResponse<LoginResponse> res, string oldRefreshToken = null)
        {
            if (res.IsSuccess)
            {
                // we directly process the token here, since we need to store it in the storage service
                var parsed = await res.ParseSuccess();
                if (parsed != null && parsed.Auth != null && parsed.Auth.Token != null)
                {
                    if (parsed.Auth.RefreshToken != null)
                    {
                        _storageService.StoreUser(parsed.Username, parsed.Auth.Token, parsed.Auth.RefreshToken);

                        await _storageService.SaveChangesAsync();
                    }


                    // TODO: Store also expiring times to logout user if he stays to long
                    (Application.Current as App)?.Login(parsed.Auth.Token);

                    // keep UI work on main thread
                    return (res, new Command(() => MainThread.BeginInvokeOnMainThread(async () => await (Application.Current as App).SwitchToMainApp())));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(oldRefreshToken))
                {
                    _storageService.RemoveUserByRefreshToken(oldRefreshToken);
                }
            }

            return (res, null);
        }

        /// <summary>
        /// Login a user to the application using a token and a refresh token
        /// </summary>
        /// <param name="token">The token of the user</param>
        /// <param name="refreshToken">The refresh token of the user</param>
        /// <returns>The Response, The command to switch to main app</returns>
        public async Task<(HTTPResponse<LoginResponse>, ICommand)> LoginAsyncWithToken(string token, string refreshToken)
        {
            var data = new RefreshRequest
            {
                Token = token,
                RefreshToken = refreshToken
            };
            var res = await _userAPIService.RefreshAsync(data);
            return await _handleLoginResponse(res, refreshToken);
        }

        /// <summary>
        /// Login a user to the application
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <param name="rememberMe">Whether the user should be remembered for future logins</param>
        /// <returns>The Response, The command to switch to main app</returns>
        public async Task<(HTTPResponse<LoginResponse>, ICommand)> LoginAsync(string username, string password, bool rememberMe)
        {
            var data = new LoginRequest
            {
                Username = username,
                Password = password,
                RememberMe = rememberMe
            };
            var res = await _userAPIService.LoginAsync(data);
            return await _handleLoginResponse(res);
        }

        /// <summary>
        /// Logout a user from the application
        /// </summary>
        /// <returns>a ICommand to be run when ready to navigate away</returns>
        public async Task LogoutAsync()
        {
            (Application.Current as App)?.Logout();
            // keep UI work on main thread
            MainThread.BeginInvokeOnMainThread(async () => await (Application.Current as App).SwitchToLogin());
        }
    }
} 
