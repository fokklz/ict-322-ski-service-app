using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using System.Windows.Input;

namespace SkiServiceApp.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        public ICommand LogoutCommand { get; set; }

        public AppShellViewModel(IAuthService authService)
        {
            _authService = authService;

            LogoutCommand = new Command(Logout);
        }

        public async void Logout()
        {
            await _authService.LogoutAsync();
        }
    }
}
