using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace SkiServiceApp.ViewModels
{
    public class AppLoginViewModel : BaseNotifyHandler
    {
        private readonly IAuthService _authService;
        private readonly IStorageService _storageService;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        public ObservableCollection<StoredUserCredentials> ReversedUsers => _storageService.ReversedUsers;

        public bool HasReversedUsers { get; set; } = false;

        public bool IsLoginSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        [DependsOn(nameof(IsLoginSuccess), nameof(Message))]
        public bool IsErrorVisible => !IsLoginSuccess && !string.IsNullOrEmpty(Message);


        public string Username { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;

        public Command<StoredUserCredentials> LoginWithUserCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public AppLoginViewModel()
        {
            _authService = ServiceLocator.GetService<IAuthService>();
            _storageService = ServiceLocator.GetService<IStorageService>();
            _mainThreadInvoker = ServiceLocator.GetService<IMainThreadInvoker>();

            LoginCommand = new Command(async () => {
                // TODO: Hide Keyboard
                await Login(); 
            });
            LoginWithUserCommand = new Command<StoredUserCredentials>(async (u) => { 
                await LoginUsingCredentials(u); 
            });

            ReversedUsers_CollectionChanged(null, null);
            ReversedUsers.CollectionChanged += ReversedUsers_CollectionChanged;
        }

        private void ReversedUsers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!ReversedUsers.Any())
            {
                HasReversedUsers = false;
            }
            else
            {
                HasReversedUsers = true;
            }
        }

        public void ResetLoginState()
        {
            IsLoginSuccess = false;
            Message = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            RememberMe = true;
        }

        private async Task _handleLogin(HTTPResponse<LoginResponse> res)
        {
            if (res.IsSuccess)
            {
                IsLoginSuccess = true;
                Message = string.Empty;
            }
            else
            {
                var error = await res.ParseError();
                IsLoginSuccess = false;
                Message = Localization.Instance.GetResource($"Backend.Error.{error?.MessageCode ?? "Server Error"}");
            }
        }

        public async Task LoginUsingCredentials(StoredUserCredentials credentials)
        {
            var res = await _authService.LoginAsyncWithToken(credentials.Token, credentials.RefreshToken);
            await _handleLogin(res);
        }

        public async Task Login()
        {
            var res = await _authService.LoginAsync(Username, Password, RememberMe);
            await _handleLogin(res);
        }
    }
}
