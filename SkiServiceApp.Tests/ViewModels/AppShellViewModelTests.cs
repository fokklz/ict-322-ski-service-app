using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Tests.ViewModels
{
    [Collection("Docker Collection")]
    public class AppShellViewModelTests
    {
        public Environment _environment;

        public AppShellViewModelTests()
        {
            _environment = new Environment().UseAuth();

            _environment.AddService<IUserAPIService>(new UserAPIService(_environment.ConfigurationMock.Object));
            _environment.AddService<IAuthService>(new AuthService(ServiceLocator.GetService<IUserAPIService>(), ServiceLocator.GetService<IStorageService>()));
        }

        private AppShellViewModel CreateViewModel()
        {
            return new AppShellViewModel(ServiceLocator.GetService<IAuthService>());
        }

        [Fact]
        public async Task Logout_UpdatesAuthManager()
        {
            // Arrange
            var model = CreateViewModel();

            // Act
            await model.Logout(true);

            // Assert
            Assert.False(AuthManager.IsLoggedIn);
            Assert.Null(AuthManager.UserId);
        }

    }
}
