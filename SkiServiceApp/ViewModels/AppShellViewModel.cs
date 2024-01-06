using SkiServiceApp.Common;
using SkiServiceApp.Components.Dialogs;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Services;
using System.Windows.Input;

namespace SkiServiceApp.ViewModels
{
    public class AppShellViewModel : BaseNotifyHandler
    {
        private readonly IAuthService _authService;

        public ICommand LogoutCommand { get; set; }

        public AppShellViewModel(IAuthService authService)
        {
            _authService = authService;

            LogoutCommand = new Command(async () => await Logout());
        }

        public async Task Logout(bool force = false)
        {
            await _authService.LogoutAsync(force);
        }
    }
}