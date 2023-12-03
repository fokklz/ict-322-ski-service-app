using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceModels.DTOs.Requests;
using System.Diagnostics;
using System.Windows.Input;

namespace SkiServiceApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AppLoginViewModel : BaseViewModel
    {
        private readonly IUserAPIService _userAPIService;
        public ICommand LoginCommand { get; set; }

        public bool IsLoginSuccess { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;
        public bool IsErrorVisible => !IsLoginSuccess;

        public AppLoginViewModel(IUserAPIService userService)
        {
            _userAPIService = userService;

            LoginCommand = new Command(Login);
        }

        public void ResetLoginState()
        {
            IsLoginSuccess = false;
            Message = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            RememberMe = true;
        }


        public async void Login()
        {
            var res = await _userAPIService.LoginAsync(new LoginRequest()
            {
                Username = Username,
                Password = Password,
                RememberMe = RememberMe
            });

            Debug.WriteLine($"Login war {res.IsSuccess} - {res.StatusCode}");
            if (res.IsSuccess)
            {
                IsLoginSuccess = true;
                Message = "Login successful";
                (Application.Current as App).SwitchToMainApp();
            }
            else
            {
                var error = await res.ParseError();
                IsLoginSuccess = false;
                Message = error?.MessageCode ?? "Server Error";
            }
        }
    }
}
