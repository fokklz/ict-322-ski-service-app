using Moq;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Helpers;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Tests.ViewModels
{
    [Collection("Docker Collection")]
    public class SettingsViewModelTests : IDisposable
    {
        private Environment _environment;

        public SettingsViewModelTests()
        {
            _environment = new Environment().UseAuth();

            _environment.AddService<IUserAPIService>(new UserAPIService(_environment.ConfigurationMock.Object));
            _environment.AddService<IAuthService>(new AuthService(ServiceLocator.GetService<IUserAPIService>(), ServiceLocator.GetService<IStorageService>()));
        }

        private SettingsViewModel CreateViewModel()
        {
            return new SettingsViewModel(ServiceLocator.GetService<IUserAPIService>(), ServiceLocator.GetService<IAuthService>());
        }

        [Fact]
        public async Task ChangeTheme_UpdatesTheme()
        {
            // Arrange
            var model = CreateViewModel();
            var expectedTheme = "Dark";

            // Act
            model.SelectedTheme = new PickerItem<string> { DisplayText = Localization.Instance.SettingsPage_Theme_Dark, BackgroundValue = expectedTheme };

            // Assert
            var actualTheme = SettingsManager.PreferencesAPI.Get($"{SettingsKey.Theme}.{AuthManager.UserId}", "System");
            Assert.Equal(expectedTheme, actualTheme);
        }

        [Fact]
        public async Task ChangeLanguage_UpdatesLanguage()
        {
            // Arrange
            var model = CreateViewModel();
            var expectedLanguage = "English";

            // Act
            model.SelectedLanguage = expectedLanguage;

            // Assert
            var actualLanguage = SettingsManager.PreferencesAPI.Get($"{SettingsKey.Language}.{AuthManager.UserId}", "Deutsch");
            Assert.Equal(expectedLanguage, actualLanguage);
        }

        [Fact]
        public async Task ChangeCancelInListView_UpdatesCancelInListView()
        {
            // Arrange
            var model = CreateViewModel();
            var expectedOption = true;

            // Act
            model.CancelInListView = expectedOption;

            // Assert
            var actualOption = SettingsManager.PreferencesAPI.Get($"{SettingsKey.CancelInListView}.{AuthManager.UserId}", false);
            Assert.Equal(expectedOption, actualOption);
        }

        [Fact]
        public async Task ChangeAlwaysSaveLogin_UpdatesAlwaysSaveLogin()
        {
            // Arrange
            var model = CreateViewModel();
            var expectedOption = true;

            // Act
            model.AlwaysSaveLogin = expectedOption;

            // Assert
            var actualOption = SettingsManager.PreferencesAPI.Get($"{SettingsKey.AlwaysSaveLogin}.{AuthManager.UserId}", false);
            Assert.Equal(expectedOption, actualOption);
        }

        [Fact]
        public async Task LogoutOnAllDevices_CallsEventAndRemovesUser()
        {
            // Arrange
            var model = CreateViewModel();
            var eventCalled = 0;
            var token = AuthManager.RefreshToken;
            var storageService = ServiceLocator.GetService<IStorageService>();
            AuthManager.LoginChanged += (sender, args) => { eventCalled += 1; };

            // Act
            await model.LogoutOnAllDevices();

            // Assert
            Assert.Equal(1, eventCalled);
            Assert.Null(storageService.ReversedUsers.FirstOrDefault(x => x.RefreshToken == token));
        }

        public void Dispose()
        {
            _environment.Dispose();
        }
    }
}
