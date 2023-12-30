using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Events;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceApp.Services;
using System.Collections.ObjectModel;

namespace SkiServiceApp.ViewModels
{
    public class SettingsViewModel : BaseNotifyHandler
    {
        private readonly IUserAPIService _userAPIService;
        private readonly IAuthService _authService;

        public ObservableCollection<PickerItem<string>> Themes { get; set; } = new ObservableCollection<PickerItem<string>>();
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string>(Localization.LanguageMap.Keys);

        [OnChangedMethod(nameof(ChangeTheme))]
        public PickerItem<string> SelectedTheme { get; set; }

        [OnChangedMethod(nameof(ChangeLanguage))]
        public string SelectedLanguage { get; set; } = SettingsService.Language;

        [OnChangedMethod(nameof(ChangeCancelInListView))]
        public bool CancelInListView { get; set; } = SettingsService.CancelInListView;

        [OnChangedMethod(nameof(ChangeAlwaysSaveLogin))]
        public bool AlwaysSaveLogin { get; set; } = SettingsService.AlwaysSaveLogin;

        public Command LogoutOnAllDevicesCommand { get; private set; }

        public SettingsViewModel(IUserAPIService userAPIService, IAuthService authService)
        {
            _userAPIService = userAPIService;
            _authService = authService;

            LogoutOnAllDevicesCommand = new Command(LogoutOnAllDevices);

            // set theme dropdown to current theme
            LanguageChanged(null, null);
            Localization.LanguageChanged += LanguageChanged;
        }

        private void LanguageChanged(object? sender, LanguageChangedEventArgs? args)
        {
            Themes.Clear();
            foreach (var item in Localization.Instance.ThemeDropdown)
            {
                Themes.Add(item);
            }
            SelectedTheme = Themes.Where(x => x.BackgroundValue == SettingsService.Theme).First();
        }

        private async void LogoutOnAllDevices()
        {
            await _userAPIService.RevokeAsync();
            await _authService.LogoutAsync(true);
        }

        private void ChangeTheme()
        {
            if(SelectedTheme == null) return;
            SettingsService.SetTheme(SelectedTheme.BackgroundValue);
        }

        private void ChangeLanguage()
        {
            SettingsService.SetLanguage(SelectedLanguage);
        }

        private void ChangeCancelInListView()
        {
            SettingsService.SetCancelInListView(CancelInListView);
        }

        private void ChangeAlwaysSaveLogin()
        {
            SettingsService.SetAlwaysSaveLogin(AlwaysSaveLogin);
        }
    }
}
