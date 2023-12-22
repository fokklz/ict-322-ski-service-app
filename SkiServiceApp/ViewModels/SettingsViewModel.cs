using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using SkiServiceApp.Resources.Helper;

namespace SkiServiceApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserAPIService _userAPIService;
        private readonly IAuthService _authService;

        public ObservableCollection<string> Themes { get; set; } = new ObservableCollection<string> { "System", "Dark", "Light" };
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string> { "عربي", "Deutsch", "English", "Español", "Français", "Italiano", "Nederlands", "Polski", "Português", "Русский", "Türkçe" };
        [OnChangedMethod(nameof(ChangeTheme))]
        public string SelectedTheme { get; set; } = Preferences.Get("Theme", "System");

        [OnChangedMethod(nameof(ChangeLanguage))]
        public string SelectedLanguage { get; set; } = Preferences.Get("Language", "Deutsch");

        [OnChangedMethod(nameof(ChangeCancelInListView))]
        public bool CancelInListView { get; set; } = Preferences.Get("CancelInListView", false);

        [OnChangedMethod(nameof(ChangeAlwaysSaveLogin))]
        public bool AlwaysSaveLogin { get; set; } = Preferences.Get("AlwaysSaveLogin", false);

        public ICommand LogoutOnAllDevicesCommand { get; private set; }

        public SettingsViewModel(IUserAPIService userAPIService, IAuthService authService)
        {
            _userAPIService = userAPIService;
            _authService = authService;

            LogoutOnAllDevicesCommand = new Command(LogoutOnAllDevices);
        }

        private async void LogoutOnAllDevices()
        {
            await _userAPIService.RevokeAsync();
            await _authService.LogoutAsync();
        }

        private void ChangeTheme()
        {
            Preferences.Set("Theme", SelectedTheme);
            var app = Application.Current;
            if (app != null)
            {
                app.UserAppTheme = SelectedTheme switch
                {
                    "Dark" => AppTheme.Dark,
                    "Light" => AppTheme.Light,
                    _ => AppTheme.Unspecified
                };
            }
        }

        private void ChangeLanguage()
        {
            Preferences.Set("Language", SelectedLanguage);
            var code = LanguageCodeHelper.GetLanguageCode(SelectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(code);
        }

        private void ChangeCancelInListView()
        {
            Preferences.Set("CancelInListView", CancelInListView);
        }

        private void ChangeAlwaysSaveLogin()
        {
            Preferences.Set("AlwaysSaveLogin", AlwaysSaveLogin);
        }
    }
}
