using Moq;
using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Helpers;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
using SkiServiceApp.Tests.Util;
using SkiServiceApp.ViewModels;
using SkiServiceModels.DTOs.Responses;
using System.Net;
using System.Net.Http;
using Xunit;

namespace SkiServiceApp.Tests.Models
{
    [Collection("Docker Collection")]
    public class SettingsViewModelTests
    {
        private readonly SettingsViewModel _viewModel;
        private readonly UserAPIService _userApiService;
        private readonly AuthService _authService;

        public SettingsViewModelTests()
        {
            // login to allow settings changes
            AuthHelper.Login();
            new Localization();

            var configurationMock = new Mock<IConfiguration>();
            _userApiService = new UserAPIService(configurationMock.Object);

            var storageServiceMock = new Mock<IStorageService>();
            _authService = new AuthService(_userApiService, storageServiceMock.Object);

            _viewModel = new SettingsViewModel(_userApiService, _authService);
        }

        [Fact]
        public void ChangeTheme_UpdatesTheme()
        {
            // Arrange
            var expectedTheme = "Dark";

            // Act
            _viewModel.SelectedTheme = new PickerItem<string> { DisplayText = Localization.Instance.SettingsPage_Theme_Dark, BackgroundValue = expectedTheme };

            // Assert
            var actualTheme = Preferences.Get($"{SettingsKey.Theme}.{AuthManager.UserId}", "System");
            Assert.Equal(expectedTheme, actualTheme);
        }

        [Fact]
        public void ChangeLanguage_UpdatesLanguage()
        {
            // Arrange
            var expectedLanguage = "English";

            // Act
            _viewModel.SelectedLanguage = expectedLanguage;

            // Assert
            var actualLanguage = Preferences.Get($"{SettingsKey.Language}.{AuthManager.UserId}", "Deutsch");
            Assert.Equal(expectedLanguage, actualLanguage);
        }

        [Fact]
        public void ChangeCancelInListView_UpdatesCancelInListView()
        {
            // Arrange
            var expectedOption = true;

            // Act
            _viewModel.CancelInListView = expectedOption;

            // Assert
            var actualOption = Preferences.Get($"{SettingsKey.CancelInListView}.{AuthManager.UserId}", false);
            Assert.Equal(expectedOption, actualOption);
        }

        [Fact]
        public void ChangeAlwaysSaveLogin_UpdatesAlwaysSaveLogin()
        {
            // Arrange
            var expectedOption = true;

            // Act
            _viewModel.AlwaysSaveLogin = expectedOption;

            // Assert
            var actualOption = Preferences.Get($"{SettingsKey.AlwaysSaveLogin}.{AuthManager.UserId}", false);
            Assert.Equal(expectedOption, actualOption);
        }

        [Fact]
        public void LogoutOnAllDevices_CallsService()
        {
            var eventCalled = 0;
            AuthManager.LoginChanged += (sender, args) => { eventCalled += 1; };

            // Act
            _viewModel.LogoutOnAllDevicesCommand.Execute(null);

            // Assert
            Assert.Equal(1, eventCalled);
        }
    }
}
