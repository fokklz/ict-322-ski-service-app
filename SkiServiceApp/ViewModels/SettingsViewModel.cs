using SkiServiceApp.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SkiServiceApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ObservableCollection<string> _themes = new ObservableCollection<string> { "System", "Dark", "Light" };
        private string _selectedTheme;
        private bool _cancelInListView;
        private bool _alwaysSaveLogin;
        private bool _askBeforeLogout;

        public ObservableCollection<string> Themes
        {
            get => _themes;
            set => SetProperty(ref _themes, value);
        }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        public bool CancelInListView
        {
            get => _cancelInListView;
            set => SetProperty(ref _cancelInListView, value);
        }

        public bool AlwaysSaveLogin
        {
            get => _alwaysSaveLogin;
            set => SetProperty(ref _alwaysSaveLogin, value);
        }

        public bool AskBeforeLogout
        {
            get => _askBeforeLogout;
            set => SetProperty(ref _askBeforeLogout, value);
        }

        public ICommand LogoutOnAllDevicesCommand { get; private set; }

        public SettingsViewModel()
        {
            LogoutOnAllDevicesCommand = new Command(LogoutOnAllDevices);
        }

        private void LogoutOnAllDevices()
        {
            // Logout
        }
    }
}
